﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBloggingPlatform.Data;
using TaskBloggingPlatform.Dto.User;
using TaskBloggingPlatform.Models.Entities;

namespace TaskBloggingPlatform.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BlogDBContext blogDBContext;
        public UserController(BlogDBContext blogDBContext)
        {
            this.blogDBContext = blogDBContext;
        }
        [Admin]
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = blogDBContext.Users.ToList();
            return Ok(users);
        }
        [HttpGet("GetUser/{Id}")]
        public IActionResult GetUser(int Id)
        {
            var user = blogDBContext.Users.Where(u => u.Id == Id).FirstOrDefault();
            if (user == null)
                return BadRequest("User not found");
            return Ok(user);
        }
        [HttpPost("FollowUser")]
        public IActionResult Follow([FromBody] CreateFollow follow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out int FollowerId);
                var newFollow = new Follow()
                {
                    FollowerId = FollowerId,
                    FollowingId = follow.FollowingId,
                };
                blogDBContext.Follows.Add(newFollow);
                blogDBContext.SaveChanges();
                return Ok(newFollow);
            }
            catch (DbUpdateException dbEx)
            {
                // Check for unique constraint violation
                if (dbEx.InnerException != null)
                {
                    string innerMessage = dbEx.InnerException.Message;
                    if (innerMessage.Contains("PK_Follow"))
                    {
                        return BadRequest("This user already you have follow");
                    }
                    if (innerMessage.Contains("CHK_FollowerNotFollowing"))
                    {
                        return BadRequest("Can't you follow your self");
                    }
                    if (innerMessage.Contains("FK_Follow_Follower"))
                    {
                        return BadRequest("This user not Found");
                    }
                    if (innerMessage.Contains("FK_Follow_Following"))
                    {
                        return BadRequest("This user you wanna follow not found");
                    }
                }
                return BadRequest(dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("UnFollowUser")]
        public IActionResult UnFollow([FromBody] CreateFollow unfollow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out int FollowerId);
                var unFollow = blogDBContext.Follows.Where(f => f.FollowerId == FollowerId && f.FollowingId == unfollow.FollowingId).FirstOrDefault();
                if (unFollow == null) return BadRequest("You unfollow this person");
                blogDBContext.Follows.Remove(unFollow);
                blogDBContext.SaveChanges();
                return Ok(unFollow);
            }
            catch (DbUpdateException dbEx)
            {
                // Check for unique constraint violation
                if (dbEx.InnerException != null)
                {
                    string innerMessage = dbEx.InnerException.Message;
                    if (innerMessage.Contains("PK_Follow"))
                    {
                        return BadRequest("This user already you have follow");
                    }
                    if (innerMessage.Contains("CHK_FollowerNotFollowing"))
                    {
                        return BadRequest("Can't you follow your self");
                    }
                    if (innerMessage.Contains("FK_Follow_Follower"))
                    {
                        return BadRequest("This user not Found");
                    }
                    if (innerMessage.Contains("FK_Follow_Following"))
                    {
                        return BadRequest("This user you wanna follow not found");
                    }
                }
                return BadRequest(dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetFollowedPosts")]
        public IActionResult GetFollowedPosts()
        {
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out int FollowerId);
            var user = blogDBContext.Users.Where(u => u.Id == FollowerId).FirstOrDefault();
            if (user == null)
                return BadRequest("User not found");
            var followingsIds = blogDBContext.Follows.Where(f => f.FollowerId == FollowerId).Select(f => f.FollowingId).ToList();
            var posts = blogDBContext.Posts.Where(p => followingsIds.Contains(p.UserId));
            return Ok(posts);
        }
        [HttpGet("GetFollowers")]
        public IActionResult GetFollowers()
        {
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out int FollowerId);
            var followerIds = blogDBContext.Follows.Where(u => u.FollowingId == FollowerId).Select(u => u.FollowerId).ToList();
            var followers = blogDBContext.Users.Where(u => followerIds.Contains(u.Id)).Include(u => u.Posts).Select(u => new { u.Posts, u.UserName, u.Email, u.Id }).ToList();
            return Ok(followers);
        }
        [HttpGet("GetFollowings")]
        public IActionResult GetFollowings()
        {
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out int FollowerId);
            var followingIds = blogDBContext.Follows.Where(u => u.FollowerId == FollowerId).Select(u => u.FollowingId).ToList();
            var followings = blogDBContext.Users.Where(u => followingIds.Contains(u.Id)).Include(u => u.Posts).Select(u => new { u.Posts, u.UserName, u.Email, u.Id }).ToList();
            return Ok(followings);
        }
    }
}