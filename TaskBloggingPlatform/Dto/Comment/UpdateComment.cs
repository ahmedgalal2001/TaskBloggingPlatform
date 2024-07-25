using System.ComponentModel.DataAnnotations;

namespace TaskBloggingPlatform.Dto.Comment
{
    public class UpdateComment
    {
        [Required(ErrorMessage = "Comment text is required.")]
        public string CommentText { get; set; }
    }
}
