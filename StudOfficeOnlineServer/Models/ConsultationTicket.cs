using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudOfficeOnlineServer.Models
{
    public class ConsultationTicket
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int? StudentId { get; set; }
        public Student? Student { get; set; }
    }
}
