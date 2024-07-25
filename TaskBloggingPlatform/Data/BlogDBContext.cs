using Microsoft.EntityFrameworkCore;
using TaskBloggingPlatform.Models.Entities;

namespace TaskBloggingPlatform.Data
{
    public class BlogDBContext : DbContext
    {
        public BlogDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Constraints 
            // Add composite primary key 
            modelBuilder.Entity<Follow>()
                .HasKey(f => new { f.FollowerId, f.FollowingId })
                .HasName("PK_Follow");

            // Add unique constraint to ensure that only one username exists 
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("UX_User_UserName");

            // Add unique constraint to ensure that only one email exists 
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("UX_User_Email");

            // Add check constraint to ensure FollowerId is not equal to FollowingId
            modelBuilder.Entity<Follow>()
                .HasCheckConstraint("CHK_FollowerNotFollowing", "[FollowerId] <> [FollowingId]");

            #endregion

            #region Relationships

            // Follow relationships
            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Followings)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Follow_Follower");

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Follow_Following");

            // Post relationships
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Post_User");

            // Comment relationships
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.CommenterId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Comment_User");

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Comment_Post");

            #endregion
        }

    }
}
