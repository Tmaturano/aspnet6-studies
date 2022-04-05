using Blog.Data;
using Blog.Dtos;
using Blog.Extensions;
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
        public async Task<IActionResult> GetAsync() => Ok(new ResultDto<List<Category>>(await _context.Categories.AsNoTracking().ToListAsync()));

        [HttpGet("v1/categories/{id}", Name = "GetCategory")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
                return NotFound(new ResultDto<Category>("Content not found"));

            return Ok(new ResultDto<Category>(category));
        }

        [HttpPost("v1/categories", Name = "CreateCategory")]
        public async Task<IActionResult> PostAsync([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultDto<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category
                {
                    Id = 0,
                    Name = createCategoryDto.Name,
                    Slug = createCategoryDto.Slug.ToLower(),
                };

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return CreatedAtRoute("GetCategory", new { id = category.Id }, new ResultDto<Category>(category));
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
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] UpdateCategoryDto categoryToUpdate)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
                return NotFound(new ResultDto<Category>("Content not found"));

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
                return NotFound(new ResultDto<Category>("Content not found"));

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
