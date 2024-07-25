using System.ComponentModel.DataAnnotations;

namespace TaskBloggingPlatform.Dto.Post
{
    public class UpdatePost
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
    }
}
