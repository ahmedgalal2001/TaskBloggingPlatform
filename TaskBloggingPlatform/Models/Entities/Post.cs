using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskBloggingPlatform.Models.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public  string  Title { get; set; }
        [Required]
        public  string Content { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
