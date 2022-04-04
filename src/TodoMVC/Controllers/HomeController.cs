using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoMVC.Data;
using TodoMVC.Models;

namespace TodoMVC.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<IActionResult> Get() => Ok(await _context.Todos.AsNoTracking().ToListAsync());

        [HttpGet("{id}", Name = "GetTodo")]
        public async Task<IActionResult> GetById(int id)
        {
            var todo = await _context.Todos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return todo is null ? NotFound() : Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetTodo", new { Id = todo.Id }, todo);
        }

        [HttpPut("id")]
        public async Task<IActionResult> Put(int id, [FromBody] Todo todo)
        {
            var model = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (model is null)
                return NotFound();

            model.Title = todo.Title;
            model.Done = todo.Done;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var model = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (model is null)
                return NotFound();

            _context.Todos.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
