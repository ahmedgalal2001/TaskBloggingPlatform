using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskBloggingPlatform.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Follow> Followers { get; set; }
        public virtual ICollection<Follow> Followings { get; set; }
    }
}
