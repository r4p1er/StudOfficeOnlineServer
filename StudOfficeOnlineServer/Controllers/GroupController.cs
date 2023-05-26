using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudOfficeOnlineServer.Models;

namespace StudOfficeOnlineServer.Controllers
{
    [ApiController]
    [Route("api/groups")]
    public class GroupController : ControllerBase
    {
        private readonly DBContext _ctx;

        public GroupController(DBContext ctx, IConfiguration configuration)
        {
            _ctx = ctx;
        }
        
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var allGroups = await _ctx.Groups.ToListAsync();

            return Ok(allGroups);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            var group = new Group
            {
                Name = name
            };

            await _ctx.Groups.AddAsync(group);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), group);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var group = await _ctx.Groups.SingleOrDefaultAsync(x => x.Id == id);

            return Ok(group);
        }
        
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromQuery]string newName)
        {
            var group = await _ctx.Groups.SingleOrDefaultAsync(x => x.Id == id);
            
             if (group == null)
                BadRequest("Group cannot be found.");

            if (!string.IsNullOrEmpty(newName))
                group.Name = newName;

            return Ok(group);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _ctx.Groups.SingleOrDefaultAsync(x => x.Id == id);

            if (group == null)
                BadRequest("Group cannot be found.");
            
            _ctx.Groups.Remove(group);
            await _ctx.SaveChangesAsync();

            return NoContent();
        }
    }
}