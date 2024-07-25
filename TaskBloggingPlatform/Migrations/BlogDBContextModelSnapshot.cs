﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskBloggingPlatform.Data;

#nullable disable

namespace TaskBloggingPlatform.Migrations
{
    [DbContext(typeof(BlogDBContext))]
    partial class BlogDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TaskBloggingPlatform.Models.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CommenterId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CommenterId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("TaskBloggingPlatform.Models.Entities.Follow", b =>
                {
                    b.Property<int>("FollowerId")
                        .HasColumnType("int");

                    b.Property<int>("FollowingId")
                        .HasColumnType("int");

                    b.HasKey("FollowerId", "FollowingId")
                        .HasName("PK_Follow");

                    b.HasIndex("FollowingId");

                    b.ToTable("Follows", t =>
                        {
                            t.HasCheckConstraint("CHK_FollowerNotFollowing", "[FollowerId] <> [FollowingId]");
                        });
                });

            modelBuilder.Entity("TaskBloggingPlatform.Models.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("TaskBloggingPlatform.Models.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("UX_User_Email");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasDatabaseName("UX_User_UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TaskBloggingPlatform.Models.Entities.Comment", b =>
                {
                    b.HasOne("TaskBloggingPlatform.Models.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("CommenterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Comment_User");

                    b.HasOne("TaskBloggingPlatform.Models.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Comment_Post");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskBloggingPlatform.Models.Entities.Follow", b =>
                {
                    b.HasOne("TaskBloggingPlatform.Models.Entities.User", "Follower")
                        .WithMany("Followings")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Follow_Follower");

                    b.HasOne("TaskBloggingPlatform.Models.Entities.User", "Following")
                        .WithMany("Followers")
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Follow_Following");

                    b.Navigation("Follower");

                    b.Navigation("Following");
                });

            modelBuilder.Entity("TaskBloggingPlatform.Models.Entities.Post", b =>
                {
                    b.HasOne("TaskBloggingPlatform.Models.Entities.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Post_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskBloggingPlatform.Models.Entities.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("TaskBloggingPlatform.Models.Entities.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Followers");

                    b.Navigation("Followings");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
