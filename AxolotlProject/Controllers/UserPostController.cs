using AxolotlProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace AxolotlProject.Controllers
{
    public class UserPostController : Controller
    {
        private readonly ILogger<UserPostController> _logger;

        //DB template
        static User? testUser;
        static UserPost? testPost;
        static Comment? testComment1;
        static Comment? testComment2;

        public UserPostController(ILogger<UserPostController> logger)
        {
            _logger = logger;

            //DB template
            if (testUser == null)
            {
                testComment1 = new Comment()
                {
                    Id = Guid.NewGuid(),
                    Content = "Comment #1 text",
                    CommentMarks = new List<CommentMark>()
                };
                testComment2 = new Comment()
                {
                    Id = Guid.NewGuid(),
                    Content = "Comment #2 text",
                    CommentMarks = new List<CommentMark>()
                };
                testPost = new UserPost()
                {
                    Id = Guid.NewGuid(),
                    Heading = "Post heading",
                    Content = "Post Content",
                    PostCategory = PostCategory.Economy,
                    Comments = new List<Comment>()
                {
                    testComment1, testComment2
                },
                    Marks = new List<PostMark>(),
                    Tags = new List<string>()
                {
                    "tag #1", "tag #2"
                }
                };
                testUser = new User()
                {
                    Login = "UserLogin228",
                    UserName = "Tester",
                    UserSurname = "SutTester",
                    Posts = new List<UserPost>()
                {
                    testPost
                },
                    Comments = new List<Comment>()
                {
                    testComment1, testComment2
                }
                };
                testComment1.PostId = testPost.Id;
                testComment1.Post = testPost;
                testComment2.PostId = testPost.Id;
                testComment2.Post = testPost;
                testComment1.UserId = testUser.Id;
                testComment1.User = testUser;
                testComment2.UserId = testUser.Id;
                testComment2.User = testUser;
                testPost.User = testUser;
            }
        }
        public IActionResult Index()
        {
            return RedirectToAction("CreatePost");
        }

        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShowPost([FromForm] string heading, string content, PostCategory postCategory, List<string> tags)
        {
            var post = new UserPost()
            {
                Id = Guid.NewGuid(),
                Heading = heading,
                Content = content,
                PostCategory = postCategory,
                Comments = new List<Comment>(),
                Marks = new List<PostMark>(),
                Tags = tags,
                User = testUser,
                UserId = testUser?.Id
            };

            return View(post);
        }

        [HttpGet]
        public IActionResult ShowPost([FromRoute] Guid postId)
        {
            //Get Post from DB by ID
            _ = postId;
            var post = testPost;
            return View(post);
        }

        [HttpPost]
        [Route("UserPost/CreateComment/{postId}")]
        public IActionResult CreateComment([FromForm] string commentContent,
                                           [FromRoute] Guid postId)
        {
            var comment = new Comment()
            {
                Id = Guid.NewGuid(),
                Content = commentContent,
                CommentMarks = new List<CommentMark>(),
                User = testUser,
                UserId = testUser?.Id,
                Post = testPost,
                PostId = testPost?.Id
            };
            //GET POST BY ID!
            testPost?.Comments?.Add(comment);
            testUser?.Comments?.Add(comment);
            return RedirectToAction("ShowPost");
        }

        [Route("UserPost/DeleteComment/{postId}/{commentId}")]
        public IActionResult DeleteComment([FromRoute] Guid postId,
                                           [FromRoute] Guid commentId)
        {
            //GET Comment BY ID!
            var comment = testUser?.Comments?.Find(x=>x.Id==commentId);
            comment?.DeleteSelf();
            return RedirectToAction("ShowPost");
        }
    }
}
