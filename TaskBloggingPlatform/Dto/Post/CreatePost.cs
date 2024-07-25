using System.ComponentModel.DataAnnotations;

namespace TaskBloggingPlatform.Dto.Post
{
    public class CreatePost
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }
    }
}
