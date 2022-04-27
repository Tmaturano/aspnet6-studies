using Blog.Data;
using Blog.Dtos;
using Blog.Dtos.Posts;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api")]    
    [Tags("Post")]
    public class PostController : ControllerBase
    {
        private readonly BlogDataContext _context;

        public PostController(BlogDataContext blogDataContext) => _context = blogDataContext;

        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetAsync(
       [FromQuery] int page = 0,
       [FromQuery] int pageSize = 25)
        {
            try
            {
                var count = await _context.Posts.AsNoTracking().CountAsync();
                var posts = await _context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Author)
                    .Include(x => x.Category)
                    .Select(x => new ListPostsDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Slug = x.Slug,
                        LastUpdateDate = x.LastUpdateDate,
                        Category = x.Category.Name,
                        Author = $"{x.Author.Name} ({x.Author.Email})"
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.LastUpdateDate)
                    .ToListAsync();
                return Ok(new ResultDto<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts
                }));
            }
            catch
            {
                return StatusCode(500, new ResultDto<List<Post>>("05X04 - Falha interna no servidor"));
            }
        }

        [HttpGet("v1/posts/{id:int}")]
        public async Task<IActionResult> DetailsAsync(
            [FromRoute] int id)
        {
            try
            {
                var post = await _context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Author)
                    .ThenInclude(x => x.Roles)
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (post == null)
                    return NotFound(new ResultDto<Post>("Conteúdo não encontrado"));

                return Ok(new ResultDto<Post>(post));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultDto<List<Post>>("05X04 - Falha interna no servidor"));
            }
        }

        [HttpGet("v1/posts/category/{category}")]
        public async Task<IActionResult> GetByCategoryAsync(
            [FromRoute] string category,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            try
            {
                var count = await _context.Posts.Where(x => x.Category.Slug == category).AsNoTracking().CountAsync();
                var posts = await _context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Author)
                    .Include(x => x.Category)
                    .Where(x => x.Category.Slug == category)
                    .Select(x => new ListPostsDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Slug = x.Slug,
                        LastUpdateDate = x.LastUpdateDate,
                        Category = x.Category.Name,
                        Author = $"{x.Author.Name} ({x.Author.Email})"
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.LastUpdateDate)
                    .ToListAsync();

                return Ok(new ResultDto<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts
                }));
            }
            catch
            {
                return StatusCode(500, new ResultDto<List<Post>>("05X04 - Falha interna no servidor"));
            }
        }
    }
}
