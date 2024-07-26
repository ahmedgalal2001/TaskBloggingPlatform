using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskBloggingPlatform.Data;
using TaskBloggingPlatform.Dto.User;
using TaskBloggingPlatform.Models.Entities;

namespace TaskBloggingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BlogDBContext blogDBContext;

        public AuthController(IConfiguration configuration, BlogDBContext blogDBContext)
        {
            _configuration = configuration;
            this.blogDBContext = blogDBContext;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUser loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = blogDBContext.Users.Where(u => u.UserName == loginModel.UserName && u.Password == loginModel.Password).FirstOrDefault();
            if (user == null) return BadRequest("Incorrect in password or username");
            var token = GenerateJwtToken(loginModel.UserName, user.Id);
            return Ok(new { Token = token });
        }

        [HttpPost("signup")]
        public IActionResult CreateUser([FromBody] CreateUser user)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors
                return BadRequest(ModelState);
            }
            try
            {
                var newUser = new User()
                {
                    Email = user.Email,
                    Password = user.Password,
                    UserName = user.UserName
                };
                blogDBContext.Users.Add(newUser);
                blogDBContext.SaveChanges();
                return Ok(newUser);
            }
            catch (DbUpdateException dbEx)
            {
                // Check for unique constraint violation
                if (dbEx.InnerException != null)
                {
                    string innerMessage = dbEx.InnerException.Message;

                    if (innerMessage.Contains("UX_User_Email"))
                    {
                        return BadRequest("The email address already exists.");
                    }
                    if (innerMessage.Contains("UX_User_UserName"))
                    {
                        return BadRequest("The username already exists.");
                    }
                }
                return BadRequest(dbEx.Message);
            }
            catch (Exception ex)
            {
                // Handle other types of exceptions
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJwtToken(string username, int id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("UserName", username),
                new Claim("UserId", id.ToString()),
                new Claim("Role", "User")
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
