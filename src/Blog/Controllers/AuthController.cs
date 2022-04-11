using Blog.Data;
using Blog.Dtos;
using Blog.Extensions;
using Blog.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api")]
    [Tags("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly BlogDataContext _context;

        public AuthController(ITokenService tokenService, BlogDataContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("v1/auth")]
        public async Task<IActionResult> Post([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultDto<string>(ModelState.GetErrors()));

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Slug = dto.Email.Replace("@", "-").Replace(".", "-")
            };

            var password = PasswordGenerator.Generate(25);
            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(new ResultDto<dynamic>(new
                {
                    user = user.Email,
                    password //send this password by email
                }));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultDto<string>("05X99 - This E-mail already exists"));
            }
            catch
            {
                return StatusCode(500, new ResultDto<string>("05X04 - Internal Server Error"));
            }
        }

        [HttpPost("v1/auth/login")]
        public IActionResult Login()
        {
            var token = _tokenService.GenerateToken(null);

            return Ok(token);
        }


        //[Authorize(Roles = "user")]
        //[HttpGet("v1/auth/user")]
        //public IActionResult GetUser() => Ok(User.Identity.Name);

        //[Authorize(Roles = "author")]
        //[Authorize(Roles = "admin")]
        //[HttpGet("v1/auth/author")]
        //public IActionResult GetAuthor() => Ok(User.Identity.Name);

        //[Authorize(Roles = "admin")]
        //[HttpGet("v1/auth/admin")]
        //public IActionResult GetAdmin() => Ok(User.Identity.Name);
    }
}
