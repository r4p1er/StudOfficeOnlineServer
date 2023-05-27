using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudOfficeOnlineServer.Models;
using StudOfficeOnlineServer.Models.DTOs;

namespace StudOfficeOnlineServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly DBContext _db;

        public SubjectsController(DBContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SubjectDTO>>> GetAll()
        {
            return await _db.Subjects.Select(x => new SubjectDTO { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<SubjectDTO>> GetById(int id)
        {
            var subject = await _db.Subjects.Include(x => x.Students).Include(x => x.Teachers).FirstOrDefaultAsync(x => x.Id == id);

            if (subject == null)
            {
                return NotFound(new { errors = "There is no such a subject." });
            }

            return new SubjectDTO { Id = subject.Id, Name = subject.Name };
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PostSubject(SubjectPostDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new { errors = "You should provide name." });
            }

            var subject = new Subject { Name =  dto.Name };
            await _db.AddAsync(subject);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), subject.Id, new SubjectDTO { Id = subject.Id, Name = subject.Name });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteSubject(int id)
        {
            var subject = await _db.Subjects.FirstOrDefaultAsync(x => x.Id == id);

            if (subject == null)
            {
                return NotFound(new { errors = "There is no such a subject." });
            }

            _db.Subjects.Remove(subject);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
