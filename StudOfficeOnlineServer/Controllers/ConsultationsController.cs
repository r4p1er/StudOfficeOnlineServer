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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ConsultationTicketDTO>>> GetConsultations(DateTime date)
        {
            var consults = await _db.Consultations.Where(x => x.Date == date.Date).ToListAsync();
            var result = new List<ConsultationTicketDTO>();

            foreach (var consult in consults)
            {
                var student = await _db.Students.Include(x => x.Group).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == consult.StudentId);

                if (student == null)
                {
                    return NotFound(new { errors = "There is no such a student." });
                }

                result.Add(new ConsultationTicketDTO { Name = $"{student.User!.LastName} {student.User.FirstName[0]}. {student.User.MiddleName[0]}.", Time = date.ToShortTimeString(), Group = student.Group?.Name ?? "", Course = student.Course });
            }

            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> PostConsultation(DateTime date)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Name));
            var student = await _db.Students.FirstOrDefaultAsync(x => x.UserId == user!.Id);

            if (student == null)
            {
                return NotFound(new { errors = "There is no such a student." });
            }

            if ((date.Minute != 0 && date.Minute != 15 && date.Minute != 30 && date.Minute != 45) || date.Hour >= 13 && date.Hour < 15 || date.Hour >= 16)
            {
                return BadRequest(new { errors = "Incorrect time." });
            }

            if (await _db.Consultations.AnyAsync(x => x.Date.Date == date.Date && x.Date.Hour == date.Hour && x.Date.Minute == date.Minute))
            {
                return BadRequest(new { errors = "You cannot take choose this date and time." });
            }

            var ticket = new ConsultationTicket { Date = date, StudentId = student.Id, Student = student };
            await _db.Consultations.AddAsync(ticket);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsultations), ticket);
        }
    }
}
