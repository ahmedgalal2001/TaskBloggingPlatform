using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskBloggingPlatform.Dto.Comment
{
    public class CreateComment
    {
        [Required(ErrorMessage = "Comment text is required.")]
        public string CommentText { get; set; }

        [Required(ErrorMessage = "Commenter ID is required.")]
        public int CommenterId { get; set; }

        [Required(ErrorMessage = "Post ID is required.")]
        public int PostId { get; set; }
    }
}
