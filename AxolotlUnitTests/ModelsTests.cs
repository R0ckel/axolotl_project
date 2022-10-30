using AxolotlProject.Models;

namespace AxolotlUnitTests
{
    public class ModelsTests
    {
        User testUser1;
        User testUser2;
        UserPost testPost1;
        UserPost testPost2;
        Comment testComment1;
        Comment testComment2;

        public ModelsTests()
        {
            testUser1 = new()
            {
                Login = "tester1",
                Posts = new()
            };
            testUser2 = new()
            {
                Login = "tester2",
                Posts = new()
            };
            testPost1 = new()
            {
                Id = Guid.NewGuid(),
                Heading = "Test heading",
                Content = "Some content",
                User = testUser1
            };
            testPost2 = new()
            {
                Id = Guid.NewGuid(),
                Heading = "Heading #2",
                Content = "Another content",
                User = testUser1
            };
            testComment1 = new()
            {
                Id = Guid.NewGuid(),
                User = testUser2,
                Post = testPost1,
                Content = "Comment content #1"
            };
            testComment2 = new()
            {
                Id = Guid.NewGuid(),
                User = testUser2,
                Post = testPost2,
                Content = "Comment content #2"
            };
        }

        //user -> posts tests
        [Fact]
        public void AddingPostNotUpdatesUserPosts()
        {
            testUser1.Posts = new();

            testUser1.AddPost(testPost1);
            Assert.Equal(testUser1.Posts?.First(), testPost2);
        }
        [Fact]
        public void AddingPostUpdatesUserPosts()
        {
            testUser1.Posts = new();

            testUser1.AddPost(testPost1);
            Assert.Equal(testUser1.Posts?.First(), testPost1);
        }

        [Fact]
        public void AddedUserPostIsLinked()
        {
            testUser1.Posts = new();

            testUser1.AddPost(testPost1);
            testPost1.Content = "Changed content";
            Assert.Equal(testUser1.Posts?.First(), testPost1);
        }

        [Fact]
        public void DeletingPostUpdatesUserPosts()
        {
            testUser1.Posts = new();

            testUser1.AddPost(testPost2);
            testUser1.DeletePost(testPost2.Id);
            Assert.Empty(testUser1.Posts);
        }

        [Fact]
        public void EditingPostUpdatesUserPosts()
        {
            testUser1.Posts = new();

            testUser1.AddPost(testPost1);
            UserPost outLinkPost = new()
            {
                Id = testPost1.Id,
                Heading = "Update",
                Content = "contentische",
                User = testPost1.User
            };
            testUser1.EditPost(outLinkPost);
            Assert.Equal(testUser1.Posts.First(), outLinkPost);
            Assert.NotEqual(testUser1.Posts.First(), testPost1);
        }

        [Fact]
        public void EditingNotExistingPostThrows()
        {
            testUser1.Posts = new();

            testUser1.AddPost(testPost2);
            testUser1.DeletePost(testPost2.Id);
            Assert.Throws<KeyNotFoundException>(() => testUser1
            .EditPost(testPost1));
        }

        //Post -> comments tests
        [Fact]
        public void AddingCommentUpdatesUserPost()
        {
            testPost1.Comments = new();

            testPost1.AddComment(testComment1);
            Assert.Equal(testPost1.Comments?.First(), testComment1);
        }

        [Fact]
        public void AddedCommentIsLinked()
        {
            testPost1.Comments = new();

            testPost1.AddComment(testComment2);
            testComment2.Content = "Changed content";
            Assert.Equal(testPost1.Comments?.First(), testComment2);
        }

        [Fact]
        public void DeletingCommentUpdatesUserPost()
        {
            testPost1.Comments = new();

            testPost1.AddComment(testComment2);
            testPost1.DeleteComment(testComment2.Id);
            Assert.Empty(testPost1.Comments);
        }

        [Fact]
        public void EditingCommentUpdatesUserPost()
        {
            testPost1.Comments = new();

            testPost1.AddComment(testComment1);
            Comment outLinkComment = new()
            {
                Id = testComment1.Id,
                Content = "dislike, unsubscribe",
                User = testUser1
            };
            testPost1.EditComment(outLinkComment);
            Assert.Equal(testPost1.Comments.First(), outLinkComment);
            Assert.NotEqual(testPost1.Comments.First(), testComment1);
        }
        [Fact]
        public void EditingNotExistingCommentThrows()
        {
            testPost1.Comments = new();

            testPost1.AddComment(testComment1);
            testPost1.DeleteComment(testComment1.Id);
            Assert.Throws<KeyNotFoundException>(() => testPost1.
            EditComment(testComment1));
        }

        [Fact]
        public void EditingCommentUpdatesPostsUser()
        {
            testUser1.Posts = new();
            testPost1.Comments = new();

            testPost1.AddComment(testComment1);
            testPost1.AddComment(testComment2);
            testUser1.AddPost(testPost1);
            var post = testUser1.GetPost(testPost1.Id) ?? null;
            Assert.NotNull(post);
            var comment = post?.GetComment(testComment1.Id) ?? null;
            Assert.NotNull(comment);
            comment.Content = "Testing comment text";
            Assert.Equal(comment, testUser1.GetPost(testPost1.Id)?.
                GetComment(testComment1.Id));
        }
    }
}
