using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudOfficeOnlineServer.Models;
using StudOfficeOnlineServer.Models.DTOs;

namespace StudOfficeOnlineServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly DBContext _db;
        private readonly IConfiguration _configuration;

        public TeacherController(DBContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TeacherDTO>>> GetTeachers()
        {
            return await _db.Teachers.Include(x => x.User).Include(x => x.Subjects).Select(x => TeacherToDTO(x)).ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TeacherDTO>> GetById(int id)
        {
            var teacher = await _db.Teachers.Include(x => x.User).Include(x => x.Subjects).FirstOrDefaultAsync(x => x.Id == id);

            if (teacher == null) return NotFound(new { errors = "There is no such a teacher." });

            return TeacherToDTO(teacher);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PostTeacher(TeacherPostDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest(new { errors = "You should provide email." });
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { errors = "You should provide password." });
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                return BadRequest(new { errors = "You should provide first name." });
            }

            if (string.IsNullOrWhiteSpace(dto.LastName))
            {
                return BadRequest(new { errors = "You should provide last name." });
            }

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password + _configuration["AuthOptions:PEPPER"]),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddleName = dto.MiddleName,
                Role = "Teacher"
            };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            var teacher = new Teacher
            {
                UserId = user.Id
            };
            await _db.Teachers.AddAsync(teacher);
            await _db.SaveChangesAsync();

            user.TeacherId = teacher.Id;
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), teacher.Id, TeacherToDTO(teacher));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PatchTeacher(int id, [FromBody] int[] ids)
        {
            var teacher = await _db.Teachers.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);

            if (teacher == null)
            {
                return NotFound(new { errors = "There is no such a teacher." });
            }

            for (int i = 0; i < ids.Length; ++i)
            {
                var subject = await _db.Subjects.Include(x => x.Teachers).Include(x => x.Students).FirstOrDefaultAsync(x => x.Id == i);

                if (subject != null && teacher.Subjects.All(x => x.Id != subject.Id))
                {
                    teacher.Subjects.Add(subject);
                }
            }

            _db.Update(teacher);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private TeacherDTO TeacherToDTO(Teacher teacher)
        {
            return new TeacherDTO
            {
                Id = teacher.Id,
                Email = teacher.User!.Email,
                Role = teacher.User.Role,
                FirstName = teacher.User.FirstName,
                MiddleName = teacher.User.MiddleName,
                LastName = teacher.User.LastName,
                Subjects = teacher.Subjects.Select(x => SubjectToDTO(x)).ToList()
            };
        }

        private SubjectDTO SubjectToDTO(Subject subject)
        {
            return new SubjectDTO { Id = subject.Id, Name = subject.Name };
        }
    }
}
