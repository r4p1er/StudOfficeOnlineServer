using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StudOfficeOnlineServer.Models;
using StudOfficeOnlineServer.Models.DTOs;
using System.Security.Claims;

namespace StudOfficeOnlineServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly DBContext _db;
        private readonly IConfiguration _configuration;

        public StudentsController(DBContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            var result = new List<StudentDTO>();

            foreach (var student in await _db.Students.Include(x => x.Group).Include(x => x.Faculty).ToListAsync())
            {
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == student.UserId);
                result.Add(StudentToDTO(user!, student));
            }

            return result;
        }

        [HttpGet("me")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<StudentDTO>> GetMe()
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Name));
            var student = await _db.Students.Include(x => x.Group).Include(x => x.Faculty).FirstOrDefaultAsync(x => x.Id == user!.StudentId);

            return StudentToDTO(user!, student!);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PostStudent(StudentPostDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest(new { errors = "You should specify email." });
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { errors = "You should specify password." });
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                return BadRequest(new { errors = "You should specify name." });
            }

            if (string.IsNullOrWhiteSpace(dto.LastName))
            {
                return BadRequest(new { errors = "You should specify surname." });
            }

            if (await _db.Groups.AnyAsync(x => x.Id == dto.GroupId) == false)
            {
                return BadRequest(new { errors = "You should specify correct group id." });
            }

            if (await _db.Faculties.AnyAsync(x => x.Id == dto.FacultyId) == false)
            {
                return BadRequest(new { errors = "You should specify correct faculty id." });
            }

            if (string.IsNullOrWhiteSpace(dto.EducationForm))
            {
                return BadRequest(new { errors = "You should specify correct education form." });
            }

            if (string.IsNullOrWhiteSpace(dto.Citizenship))
            {
                return BadRequest(new { errors = "You should specify citizenship." });
            }

            if (dto.EducationEnd < dto.EducationStart)
            {
                return BadRequest(new { errors = "You should specify correct education end and education start." });
            }

            if (string.IsNullOrWhiteSpace(dto.OrderNumber))
            {
                return BadRequest(new { errors = "You should specify order number." });
            }

            if (string.IsNullOrWhiteSpace(dto.EducationBase))
            {
                return BadRequest(new { errors = "You should specify correct education base." });
            }

            if (dto.Course <= 0 || dto.Course >= 7)
            {
                return BadRequest(new { errors = "You should specify correct course." });
            }

            if (string.IsNullOrWhiteSpace(dto.StudentCard))
            {
                return BadRequest(new { errors = "You should specify student card." });
            }

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password + _configuration["AuthOptions:PEPPER"]),
                Role = "Student",
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName
            };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            var student = new Student
            {
                GroupId = dto.GroupId,
                FacultyId = dto.FacultyId,
                Citizenship = dto.Citizenship,
                EducationStart = dto.EducationStart,
                EducationEnd = dto.EducationEnd,
                OrderNumber = dto.OrderNumber,
                StudentCard = dto.StudentCard,
                UserId = user.Id,
                EducationForm = dto.EducationForm,
                EducationBase = dto.EducationBase,
                Course = dto.Course
            };

            await _db.Students.AddAsync(student);
            await _db.SaveChangesAsync();

            user.StudentId = student.Id;
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudents), StudentToDTO(user, (await _db.Students.Include(x => x.Group).Include(x => x.Faculty).FirstOrDefaultAsync(x => x.Id == student.Id))!));
        } 

        private StudentDTO StudentToDTO(User user, Student student)
        {
            return new StudentDTO
            {
                Id = student.Id,
                Email = user.Email,
                Role = user.Role,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Group = new GroupDTO { Id = student.GroupId, Name = student.Group!.Name },
                Faculty = new FacultyDTO { Id = student.FacultyId, Name = student.Faculty!.Name.ToString() },
                EducationForm = student.EducationForm.ToString(),
                Citizenship = student.Citizenship,
                EducationStart = student.EducationStart,
                EducationEnd = student.EducationEnd,
                OrderNumber = student.OrderNumber,
                EducationBase = student.EducationBase.ToString(),
                Course = student.Course,
                StudentCard = student.StudentCard,
                OrderDate = student.OrderDate
            };
        }
    }
}

