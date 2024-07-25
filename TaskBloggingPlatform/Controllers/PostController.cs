using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskBloggingPlatform.Data;
using TaskBloggingPlatform.Dto.Post;
using TaskBloggingPlatform.Models.Entities;

namespace TaskBloggingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly BlogDBContext blogDBContext;
        public PostController(BlogDBContext blogDBContext)
        {
            this.blogDBContext = blogDBContext;
        }
        [HttpGet("GetPosts")]
        public IActionResult GetPosts()
        {
            var posts = blogDBContext.Posts.Include(p => p.Comments).OrderByDescending(p => p.CreationDate).ToList();
            return Ok(posts);
        }
        [HttpGet("GetPost")]
        public IActionResult GetPost(int id)
        {
            var post = blogDBContext.Posts.Where(p => p.Id == id).Include(p => p.Comments).FirstOrDefault();
            if (post == null)
                return BadRequest("Post not found");
            return Ok(post);
        }
        [HttpGet("GetUserPosts")]
        public IActionResult GetUserPosts(int UserId)
        {
            var posts = blogDBContext.Users.Where(u => u.Id == UserId).Include(p => p.Posts).ThenInclude(p => p.Comments).Select(u => u.Posts).FirstOrDefault();
            if (posts == null)
                return BadRequest("User not found");
            return Ok(posts);
        }
        [HttpPost("CreatePost")]
        public IActionResult CreatePost([FromBody] CreatePost post)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors
                return BadRequest(ModelState);
            }
            try
            {
                var newPost = new Post()
                {
                    Title = post.Title,
                    Content = post.Content,
                    UserId = post.UserId,
                };
                blogDBContext.Posts.Add(newPost);
                blogDBContext.SaveChanges();
                return Ok(newPost);
            }
            catch (DbUpdateException dbEx)
            {
                // Check for unique constraint violation
                if (dbEx.InnerException != null)
                {
                    string innerMessage = dbEx.InnerException.Message;

                    if (innerMessage.Contains("FK_Post_User"))
                    {
                        return BadRequest("This user not found");
                    }
                }
                return BadRequest(dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("UpdatePost")]
        public IActionResult UpdatePost(int UserId, int PostId, [FromBody] UpdatePost post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var oldPost = blogDBContext.Posts.Where(p => p.Id == PostId && p.UserId == UserId).FirstOrDefault();
                if (oldPost == null) return BadRequest("This post not found or not own to you");
                oldPost.Title = post.Title;
                oldPost.Content = post.Content;
                blogDBContext.SaveChanges();
                return Ok(oldPost);
            }
            catch (DbUpdateException dbEx)
            {
                // Check for unique constraint violation
                if (dbEx.InnerException != null)
                {
                    string innerMessage = dbEx.InnerException.Message;

                    if (innerMessage.Contains("FK_Post_User"))
                    {
                        return BadRequest("This user not found");
                    }
                }
                return BadRequest(dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeletePost")]
        public IActionResult DeletePost(int UserId, int PostId)
        {
            try
            {
                var oldPost = blogDBContext.Posts.Where(p => p.Id == PostId && p.UserId == UserId).FirstOrDefault();
                if (oldPost == null) return BadRequest("This post not found or not own to you");
                blogDBContext.Posts.Remove(oldPost);
                blogDBContext.SaveChanges();
                return Ok(oldPost);
            }
            catch (DbUpdateException dbEx)
            {
                // Check for unique constraint violation
                if (dbEx.InnerException != null)
                {
                    string innerMessage = dbEx.InnerException.Message;

                    if (innerMessage.Contains("FK_Post_User"))
                    {
                        return BadRequest("This user not found");
                    }
                }
                return BadRequest(dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("SearchPost")]
        public IActionResult SearchPost([FromQuery] string Title, [FromQuery] string UserName)
        {
            var query = blogDBContext.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(Title))
            {
                query = query.Where(p => p.Title.Contains(Title));
            }

            if (!string.IsNullOrEmpty(UserName))
            {
                query = query.Where(u=> u.User.UserName == UserName);
            }
            var posts = query.OrderByDescending(p => p.CreationDate).ToList();
            return Ok(posts);

        }
    }
}
