using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskBloggingPlatform.Models.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public  string CommentText { get; set; }
        [ForeignKey("User")]
        [Required]
        public int CommenterId {  get; set; }
        public User User { get; set; }
        [ForeignKey("Post")]
        [Required]
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
