using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api")]
    [Tags("Category")]
    public class CategoryController : ControllerBase
    {
        private readonly BlogDataContext _context;

        public CategoryController(BlogDataContext context) => _context = context;

        [HttpGet("v1/categories", Name = "GetCategories")]
        public async Task<IActionResult> GetAsync() => Ok(await _context.Categories.AsNoTracking().ToListAsync());

        [HttpGet("v1/categories/{id}", Name = "GetCategory")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost("v1/categories", Name = "CreateCategory")]
        public async Task<IActionResult> PostAsync([FromBody] Category category)
        {
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return CreatedAtRoute("GetCategory", new { id = category.Id }, category);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "05XE9 - Not possible to create the category");
            }
            catch (Exception)
            {
                return StatusCode(500, "05XE10 - Internal Server Error");
            }
        }

        [HttpPut("v1/categories/{id}", Name = "UpdateCategory")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] Category categoryToUpdate)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
                return NotFound();

            category.Name = categoryToUpdate.Name;
            category.Slug = categoryToUpdate.Slug;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("v1/categories/{id}", Name = "DeleteCategory")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }
    }
}
