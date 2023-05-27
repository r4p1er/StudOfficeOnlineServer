using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudOfficeOnlineServer.Models;
using StudOfficeOnlineServer.Models.DTOs;

namespace StudOfficeOnlineServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationsController : ControllerBase
    {
        private readonly DBContext _db;

        ConsultationsController(DBContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Roles = "1")]
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
    }
}
