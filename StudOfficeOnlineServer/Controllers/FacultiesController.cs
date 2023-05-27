using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudOfficeOnlineServer.Models;
using StudOfficeOnlineServer.Models.DTOs;

namespace StudOfficeOnlineServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultiesController : ControllerBase
    {
        private readonly DBContext _db;

        public FacultiesController(DBContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FacultyDTO>>> GetAll()
        {
            return await _db.Faculties.Select(x => new FacultyDTO { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<FacultyDTO>> GetById(int id)
        {
            var faculty = await _db.Faculties.FirstOrDefaultAsync(x => x.Id == id);

            if (faculty == null) return NotFound(new { errors = "There is no such a faculty." });

            return new FacultyDTO { Id = faculty.Id, Name = faculty.Name };
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PostFaculty(FacultyPostDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new { errors = "You should provide name." });
            }

            var faculty = new Faculty { Name = dto.Name };
            await _db.Faculties.AddAsync(faculty);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), faculty.Id, faculty);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PatchFaculty(int id, FacultyPostDTO dto)
        {
            var faculty = await _db.Faculties.FirstOrDefaultAsync(x => x.Id == id);

            if (faculty == null) return NotFound(new { errors = "There is no such a faculty." });

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new { errors = "You should provide name." });
            }

            faculty.Name = dto.Name;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteFaculty(int id)
        {
            var faculty = await _db.Faculties.FirstOrDefaultAsync(x => x.Id == id);

            if (faculty == null) return NotFound(new { errors = "There is no such a faculty." });

            _db.Faculties.Remove(faculty);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
