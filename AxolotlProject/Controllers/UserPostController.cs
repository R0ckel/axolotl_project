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
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserPost> result = await _context.UserPosts!.ToListAsync();
            return View(result);
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
            post.User = user;
            post.UserId = user.Id;
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }
        [Authorize]
        public async Task<IActionResult> EditPost(Guid? id)
        {
            if (id == null || _context.UserPosts == null)
            {
                return NotFound();
            }

            var post = await _context.UserPosts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid? id, [Bind("Id,Heading,Content,PostCategory")] UserPost post)
        {
            if (id != post.Id) return NotFound();
            _context.Update(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [HttpPost, ActionName("DeletePost")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var post = await _context.UserPosts!.FirstOrDefaultAsync(p => p.Id == postId);
            if(user == null || post == null) return NotFound();
            _context.Remove(post);
            await _context.SaveChangesAsync();
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

            ViewBag.Comments = _context.Comments?.Where(comment => comment.PostId.Equals(postId)).ToList();
            
            return View(post);
        }
        [Authorize]
        [HttpPost, ActionName("CreateComment")]
        public async Task<IActionResult> CreateComment(string commentContent, Guid postId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var post = await _context.UserPosts!.FirstOrDefaultAsync(p => p.Id == postId);
            if(user == null || post == null) return NotFound();
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

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(Guid? id, string Content)
        {
            var comment = await _context.Comments!.FirstOrDefaultAsync(p => p.Id == id);
            if(comment == null) return NotFound();
            comment.Content = Content;
            _context.Update(comment);
            await _context.SaveChangesAsync();
            Console.WriteLine(comment.PostId);
            return RedirectToAction("ShowPost", new { postId = comment.PostId });
        }
        [Authorize]
        [HttpPost, ActionName("DeleteComment")]
        public async Task<IActionResult> DeleteComment(Guid commentId, Guid postId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var comment = await _context.Comments!.FirstOrDefaultAsync(p => p.Id == commentId);
            if(user == null || comment == null) return NotFound();
            if(comment.UserId == user.Id) {
                _context.Remove(comment);
                comment.DeleteSelf();
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowPost", new { postId = postId });
        }
    }
}
