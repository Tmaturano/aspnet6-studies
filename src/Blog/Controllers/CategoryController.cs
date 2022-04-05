using Blog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly BlogDataContext _context;

        public CategoryController(BlogDataContext context) => _context = context;

        [HttpGet("all", Name = "GetCategories")]
        public async Task<IActionResult> Get() => Ok(await _context.Categories.ToListAsync());
    }
}
