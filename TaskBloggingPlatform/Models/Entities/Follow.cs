using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskBloggingPlatform.Models.Entities
{
    public class Follow
    {
        [ForeignKey("FollowerUser")]
        public int FollowerId { get; set; }

        [ForeignKey("FollowingUser")]
        public int FollowingId { get; set; }

        public User Follower { get; set; }
        public User Following { get; set; }
    }
}
