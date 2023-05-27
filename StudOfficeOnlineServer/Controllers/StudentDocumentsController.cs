using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudOfficeOnlineServer.Models;
using StudOfficeOnlineServer.Models.DTOs;
using System.Security.Claims;

namespace StudOfficeOnlineServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentDocumentsController : ControllerBase
    {
        private readonly DBContext _db;
        private readonly IConfiguration _configuration;

        public StudentDocumentsController(DBContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> PostDocument(IFormFile file)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Name));
            var student = await _db.Students.FirstOrDefaultAsync(x => x.UserId == user!.Id);

            if (student == null)
            {
                return NotFound(new { errors = "There is no such a student." });
            }

            if (file.Length <= 0 || Path.GetExtension(file.FileName) != ".pdf" || file.Length > 64 * 1024)
            {
                return BadRequest(new { errors = "Invalid file." });
            }

            var filePath = Path.Combine(_configuration["Files:Path"]!, Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".pdf");

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var studentDocument = new StudentDocument { Student = student, StudentId = student.Id, Path = filePath };
            await _db.StudentDocuments.AddAsync(studentDocument);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<StudentDocumentDTO>>> GetDocuments()
        {
            if (User.IsInRole("Student"))
            {
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Name));
                var student = await _db.Students.FirstOrDefaultAsync(x => x.UserId == user!.Id);

                if (student == null)
                {
                    return NotFound(new { errors = "There is no such a student." });
                }

                return await _db.StudentDocuments.Where(x => x.StudentId == student.Id).Select(x => new StudentDocumentDTO { Id = x.Id, StudentId = x.StudentId, Name = Path.GetFileName(x.Path) }).ToListAsync();
            }
            else
            {
                return await _db.StudentDocuments.Select(x => new StudentDocumentDTO { Id = x.Id, StudentId = x.StudentId, Name = Path.GetFileName(x.Path) }).ToListAsync();
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<FileResult>> GetFile(int id)
        {
            var studentDocument = await _db.StudentDocuments.FirstOrDefaultAsync(x => x.Id == id);

            if (studentDocument == null)
            {
                return NotFound(new { errors = "There is no such a document." });
            }

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Name));
            
            if (User.IsInRole("Student"))
            {
                var student = await _db.Students.FirstOrDefaultAsync(x => x.UserId == user!.Id);

                if (student == null)
                {
                    return NotFound(new { errors = "There is no such a student." });
                }

                if (student.Id != studentDocument.StudentId)
                {
                    return Forbid();
                }
            }

            var bytes = System.IO.File.ReadAllBytes(studentDocument.Path);

            return File(bytes, "application/pdf", Path.GetFileName(studentDocument.Path));
        }
    }
}
