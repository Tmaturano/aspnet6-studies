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
        private readonly IEmailService _emailService;
        private readonly BlogDataContext _context;

        public AuthController(ITokenService tokenService, 
            IEmailService emailService,
            BlogDataContext context)
        {
            _tokenService = tokenService;
            _emailService = emailService;
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

                //TODO: implement the add to the UserRole table
                //var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == dto.Role);
                //if (role is not null)                
                //    await _context.UserRoles.AddAsync(new UserRole { RoleId = role.Id, UserId = user.Id });                

                await _context.SaveChangesAsync();
                await _emailService.SendAsync(user.Name, user.Email, "Welcome to the Blog!",
                    $"Your Password is <strong>{password}</strong>", 
                    "My Blog API", "blogApi@test.com");


                return Ok(new ResultDto<dynamic>(new
                {
                    user = user.Email,                    
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
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultDto<string>(ModelState.GetErrors()));

            var user = await _context
                            .Users
                            .AsNoTracking()
                            .Include(x => x.Roles)
                            .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user is null)
                return StatusCode(401, new ResultDto<string>("Invalid user or password"));

            if (!PasswordHasher.Verify(user.PasswordHash, dto.Password))
                return StatusCode(401, new ResultDto<string>("Invalid user or password"));

            try
            {

                var token = _tokenService.GenerateToken(user);

                return Ok(new ResultDto<string>(token, null));
            }
            catch
            {
                return StatusCode(500, new ResultDto<string>("05X04 - Internal Server Error"));
            }
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
