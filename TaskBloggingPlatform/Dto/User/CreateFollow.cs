using System.ComponentModel.DataAnnotations;

namespace TaskBloggingPlatform.Dto.User
{
    public class CreateFollow
    {
        [Required(ErrorMessage = "Follower Id Is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Follower Id must be greater than 0")]
        public int FollowerId { get; set; }

        [Required(ErrorMessage = "Following Id Is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Following Id must be greater than 0")]
        public int FollowingId { get; set; }
    }
}
