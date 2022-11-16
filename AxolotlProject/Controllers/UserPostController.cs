using AxolotlProject.Models;
using Microsoft.AspNetCore.Mvc;
using AxolotlProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AxolotlProject.Controllers
{
    public class UserPostController : Controller
    {
        private readonly ILogger<UserPostController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserPostController(ILogger<UserPostController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string? search, string? category, int num, bool nameSearch = false, bool authorSearch = false, bool tagsSearch = false, string? addFilt = null)
        {
            List<UserPost> result = await _context.UserPosts!.ToListAsync();
            if (category is not null)
                result = result.Where(post => post.PostCategory!.ToString().Equals(category)).ToList();
            if (search is not null)
            {
                result = result.Where(post =>
                    (nameSearch && post.Heading is not null && (post.Heading!.Trim().ToLower().Contains(search.Trim().ToLower()) || search.Trim().ToLower().Contains(post.Heading.Trim().ToLower()))) ||
                    (authorSearch && _context.Users?.Find(post.UserId) is not null && (post.User!.Login!.Trim().ToLower().Contains(search.Trim().ToLower()) || search.Trim().ToLower().Contains(post.User.Login.Trim().ToLower()))) ||
                    (tagsSearch && post.Tags.Any(t => t.Trim().ToLower().Contains(search.Trim().ToLower()) || search.Trim().ToLower().Contains(t.Trim().Remove(0, 1).ToLower()))))
                .ToList();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.ViewerId = user?.Id;
            ViewBag.Search = search;
            ViewBag.Category = category;
            ViewBag.NameSearch = nameSearch;
            ViewBag.AuthorSearch = authorSearch;
            ViewBag.TagsSearch = tagsSearch;
            ViewBag.Num = num;
            ViewBag.AddFilt = addFilt;
            ViewBag.CommentsAmount = result.ToDictionary(p => p.Id, p => _context.Comments?.Where(comment => comment.PostId.Equals(p.Id)).Count());
            ViewBag.Rating = result.ToDictionary(p => p.Id,
                                p => _context.PostsMarks?
                                        .Where(m => m.PostId.Equals(p.Id))
                                        .Select(m => m.Liked ? 1 : -1).AsEnumerable()
                                        .Aggregate(0, (x, y) => x + y));

            if (addFilt is not null)
            {
                if (addFilt.Equals("popular")) result = result.Where(p => ViewBag.Rating[p.Id] >= 5).ToList();
                else if (addFilt.Equals("commented")) result = result.Where(p => ViewBag.CommentsAmount[p.Id] > 0).ToList();
                else if (addFilt.Equals("my")) result = result.Where(p => p.UserId!.Equals(user?.Id)).ToList();
            }

            int postsAmount = 5;
            num = num < 0 ? 0 : num;
            num = num > (result.Count - 1) / postsAmount ? (result.Count - 1) / postsAmount : num;
            int left = num * postsAmount,
                right = (left + postsAmount) is int r && r < result.Count ? r : result.Count;

            return View(result.ToArray<UserPost>()[left..right]);
        }

        [Authorize]
        public IActionResult CreatePost()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([Bind("Id,Heading,Content,PostCategory")] UserPost post)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid)
            {
                post.User = user;
                post.UserId = user.Id;
                //post.CreationTime = DateTime.Now; //UPDATE DB MIGRATIONS
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        [Authorize]
        public async Task<IActionResult> EditPost(Guid? id)
        {
            var post = await _context.UserPosts!.FindAsync(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (id == null || _context.UserPosts == null || post == null)
                return NotFound();
            return user.Id == post.UserId ? View(post) : RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid? id, string Heading, string Content, PostCategory PostCategory)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var post = await _context.UserPosts!.FirstOrDefaultAsync(p => p.Id == id);
            if (id != post?.Id) return NotFound();
            if (user.Id == post?.UserId)
            {
                post.Heading = Heading;
                post.Content = Content;
                post.PostCategory = PostCategory;
                _context.Update(post);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost, ActionName("DeletePost")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var post = await _context.UserPosts!.FirstOrDefaultAsync(p => p.Id == postId);
            if (user == null || post == null) return NotFound();
            if (user.Id == post.UserId)
            {
                _context.Remove(post);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ShowPost(Guid? postId)
        {
            if (postId == null || _context.UserPosts == null)
            {
                return NotFound();
            }

            var post = await _context.UserPosts
                .FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
            {
                return NotFound();
            }

            ViewBag.PostOwner = _context.Users?.Find(post.UserId);
            ViewBag.OwnerPostsAmount = _context.UserPosts?.Where(p => p.UserId == post.UserId).Count();
            ViewBag.Comments = _context.Comments?.Where(comment => comment.PostId.Equals(postId)).ToList();
            var commentsOwners = new Dictionary<Guid, string?>();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.ViewerId = user?.Id;
            foreach (var item in ViewBag.Comments)
            {
                commentsOwners.Add(item.Id, _context.Users?.Find(item.UserId).Login);
            }
            ViewBag.CommentsOwners = commentsOwners;
            ViewBag.CommentsRating = _context.Comments?.Where(c => c.PostId.Equals(postId))
                .ToDictionary(
                        c => c.Id,
                        c => _context.CommentsMarks?
                            .Where(mark => mark.CommentId.Equals(c.Id))
                            .Select(mark => mark.Liked ? 1 : -1).AsEnumerable()
                            .Aggregate(0, (x, y) => x + y)
                        );

            ViewBag.Rating = _context.PostsMarks?
                .Where(m => m.PostId.Equals(postId))
                .Select(m => m.Liked ? 1 : -1).AsEnumerable()
                .Aggregate(0, (x, y) => x + y);

            return View(post);
        }

        [Authorize]
        [HttpPost, ActionName("CreateComment")]
        public async Task<IActionResult> CreateComment(string commentContent, Guid postId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var post = await _context.UserPosts!.FirstOrDefaultAsync(p => p.Id == postId);
            if (user == null || post == null) return NotFound();
            var comment = new Comment()
            {
                Id = Guid.NewGuid(),
                Content = commentContent,
                CommentMarks = new List<CommentMark>(),
                User = user,
                UserId = user?.Id,
                Post = post,
                PostId = post!.Id
            };
            post?.Comments?.Add(comment);
            user?.Comments?.Add(comment);
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowPost", new { postId = postId });
        }

        [Authorize]
        public async Task<IActionResult> EditComment(Guid? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return user.Id == comment.UserId ? View(comment) : RedirectToAction("ShowPost", new { postId = comment.PostId });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(Guid? id, string Content)
        {
            var comment = await _context.Comments!.FirstOrDefaultAsync(p => p.Id == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (comment == null) return NotFound();
            if (user.Id == comment.UserId)
            {
                comment.Content = Content;
                _context.Update(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowPost", new { postId = comment.PostId });
        }

        [Authorize]
        [HttpPost, ActionName("DeleteComment")]
        public async Task<IActionResult> DeleteComment(Guid commentId, Guid postId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var comment = await _context.Comments!.FirstOrDefaultAsync(p => p.Id == commentId);
            if (user == null || comment == null) return NotFound();
            if (comment.UserId == user.Id)
            {
                _context.Remove(comment);
                comment.DeleteSelf();
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowPost", new { postId = postId });
        }

        [Authorize]
        [HttpGet, ActionName("MarkComment")]
        public async Task<IActionResult> MarkComment(Guid? commentId, bool mark = true)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var comment = await _context.Comments!.FirstOrDefaultAsync(p => p.Id == commentId);
            var marks = _context.CommentsMarks;
            if (user == null || comment == null) return NotFound();
            if (marks?.FirstOrDefault(m => m.UserId.Equals(Guid.Parse(user.Id)) && m.CommentId.Equals(comment.Id)) is var currentMark
                    && currentMark is not null
                    && !currentMark.Equals(default(CommentMark)))
            {
                if (mark.Equals(currentMark.Liked))
                {
                    comment.CommentMarks?.Remove(currentMark!);
                    user.CommentMarks?.Remove(currentMark!);
                    _context.CommentsMarks?.Remove(currentMark);
                }
                else
                {
                    currentMark.Liked = mark;
                }
            }
            else
            {
                var commentMark = new CommentMark()
                {
                    User = user,
                    UserId = Guid.Parse(user.Id),
                    Comment = comment,
                    CommentId = comment.Id,
                    Liked = mark
                };
                comment.CommentMarks?.Add(commentMark);
                user.CommentMarks?.Add(commentMark);
                _context.Add(commentMark);

            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ShowPost", new { postId = comment.PostId });
        }


        [Authorize]
        [HttpGet, ActionName("MarkPost")]
        public async Task<IActionResult> MarkPost(Guid? postId, bool mark = true)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var post = await _context.UserPosts!.FirstOrDefaultAsync(p => p.Id == postId);
            var marks = _context.PostsMarks;
            if (user == null || post == null) return NotFound();
            if (marks?.FirstOrDefault(m => m.UserId.Equals(Guid.Parse(user.Id)) && m.PostId.Equals(post.Id)) is var currentMark
                    && currentMark is not null
                    && !currentMark.Equals(default(PostMark)))
            {
                if (mark.Equals(currentMark.Liked))
                {
                    post.Marks?.Remove(currentMark!);
                    user.PostMarks?.Remove(currentMark!);
                    _context.PostsMarks?.Remove(currentMark);
                }
                else
                {
                    currentMark.Liked = mark;
                }
            }
            else
            {
                var postMark = new PostMark()
                {
                    User = user,
                    UserId = Guid.Parse(user.Id),
                    Post = post,
                    PostId = post.Id,
                    Liked = mark
                };
                post.Marks?.Add(postMark);
                user.PostMarks?.Add(postMark);
                _context.Add(postMark);

            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowPost), new { postId = postId });
        }



    }
}
