using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBloggingPlatform.Data;
using TaskBloggingPlatform.Dto.Comment;
using TaskBloggingPlatform.Models.Entities;

namespace TaskBloggingPlatform.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly BlogDBContext blogDBContext;
        public CommentController(BlogDBContext blogDBContext)
        {
            this.blogDBContext = blogDBContext;
        }
        [HttpPost("CreateComment")]
        public IActionResult CreateComment([FromBody] CreateComment comment)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors
                return BadRequest(ModelState);
            }
            try
            {
                int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out int UserId);
                var newComment = new Comment()
                {
                    CommentText = comment.CommentText,
                    CommenterId = UserId,
                    PostId = comment.PostId,
                };
                blogDBContext.Comments.Add(newComment);
                blogDBContext.SaveChanges();
                return Ok(newComment);
            }
            catch (DbUpdateException dbEx)
            {
                // Check for unique constraint violation
                if (dbEx.InnerException != null)
                {
                    string innerMessage = dbEx.InnerException.Message;

                    if (innerMessage.Contains("FK_Comment_User"))
                    {
                        return BadRequest("This user not found");
                    }
                    if (innerMessage.Contains("FK_Comment_Post"))
                    {
                        return BadRequest("This post not found");
                    }
                }
                return BadRequest(dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Get all comments for a specific post
        [HttpGet("GetPostComments")]
        public IActionResult GetCommentsForPost(int postId)
        {
            var comments = blogDBContext.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .Select(c=> new { c.User.UserName , c.User.Email , c.Id , c.CommentText})
                .ToList();
            return Ok(comments);
        }

        // Get a comment by ID
        [HttpGet("GetComment")]
        public IActionResult GetComment(int id)
        {
            var comment = blogDBContext.Comments.Include(c => c.User).Select(c => new { c.User.UserName, c.User.Email, c.Id, c.CommentText }).FirstOrDefault(c => c.Id == id);
            if (comment == null)
            {
                return BadRequest("This comment not found");
            }
            return Ok(comment);
        }

        // Update a comment
        [HttpPatch("UpdateComment")]
        public IActionResult UpdateComment(int CommentId, [FromBody] UpdateComment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out int UserId);
            var oldComment = blogDBContext.Comments.FirstOrDefault(c => c.Id == CommentId && c.User.Id == UserId);
            if (oldComment == null)
            {
                return BadRequest("This comment not found or not own you");
            }

            oldComment.CommentText = comment.CommentText;
            try
            {
                blogDBContext.SaveChanges();
                return Ok(oldComment);
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null)
                {
                    string innerMessage = dbEx.InnerException.Message;
                    if (innerMessage.Contains("FK_Comment_User"))
                    {
                        return BadRequest("This user not found");
                    }
                    if (innerMessage.Contains("FK_Comment_Post"))
                    {
                        return BadRequest("This post not found");
                    }
                }
                return BadRequest(dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete a comment
        [HttpDelete("DeleteComment")]
        public IActionResult DeleteComment(int CommentId)
        {
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out int UserId);
            var comment = blogDBContext.Comments.FirstOrDefault(c => c.Id == CommentId && c.User.Id == UserId);
            if (comment == null)
            {
                return BadRequest("This comment not found or not own you");
            }

            blogDBContext.Comments.Remove(comment);
            try
            {
                blogDBContext.SaveChanges();
                return Ok(comment);
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null)
                {
                    string innerMessage = dbEx.InnerException.Message;
                    if (innerMessage.Contains("FK_Comment_User"))
                    {
                        return BadRequest("This user not found");
                    }
                    if (innerMessage.Contains("FK_Comment_Post"))
                    {
                        return BadRequest("This post not found");
                    }
                }
                return BadRequest(dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
