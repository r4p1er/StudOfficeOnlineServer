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
    public class ConsultationsController : ControllerBase
    {
        private readonly DBContext _db;

        public ConsultationsController(DBContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ConsultationTicketDTO>>> GetConsultations([FromQuery]ConsultationPostDTO dto)
        {
            var consults = await _db.Consultations.Where(x => x.Date.Year == dto.date.Year && x.Date.Month == dto.date.Month && x.Date.Day == dto.date.Day).ToListAsync();
            var result = new List<ConsultationTicketDTO>();

            foreach (var consult in consults)
            {
                var student = await _db.Students.Include(x => x.Group).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == consult.StudentId);

                if (student == null)
                {
                    return NotFound(new { errors = "There is no such a student." });
                }

                result.Add(new ConsultationTicketDTO { Name = $"{student.User!.LastName} {student.User.FirstName[0]}. {student.User.MiddleName[0]}.", Time = dto.date.ToShortTimeString(), Group = student.Group?.Name ?? "", Course = student.Course });
            }

            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> PostConsultation(ConsultationPostDTO dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Name));
            var student = await _db.Students.Include(x => x.Group).FirstOrDefaultAsync(x => x.UserId == user!.Id);

            if (student == null)
            {
                return NotFound(new { errors = "There is no such a student." });
            }

            if ((dto.date.Minute != 0 && dto.date.Minute != 15 && dto.date.Minute != 30 && dto.date.Minute != 45) || dto.date.Hour >= 13 && dto.date.Hour < 15 || dto.date.Hour >= 16)
            {
                return BadRequest(new { errors = "Incorrect time." });
            }

            var dattas = await _db.Consultations.FirstOrDefaultAsync(x => x.Date.Date == dto.date.Date);
            dattas = await _db.Consultations.FirstOrDefaultAsync(x => x.Date.Hour == dto.date.Hour);
            dattas = await _db.Consultations.FirstOrDefaultAsync(x => x.Date.Minute == dto.date.Minute);

            if (dattas != null) return BadRequest(new { errors = "You cannot take choose this date and time." });

            var ticket = new ConsultationTicket { Date = dto.date, StudentId = student.Id, Student = student };
            await _db.Consultations.AddAsync(ticket);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsultations), new ConsultationTicketDTO { Name = $"{student.User!.LastName} {student.User.FirstName[0]}. {student.User.MiddleName[0]}.", Time = dto.date.ToShortTimeString(), Group = student.Group?.Name ?? "", Course = student.Course });
        }
    }
}
