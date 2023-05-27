using System.ComponentModel.DataAnnotations.Schema;

namespace StudOfficeOnlineServer.Models.DTOs
{
    public class StudentPostDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public int FacultyId { get; set; }
        public string EducationForm { get; set; } = string.Empty;
        public string Citizenship { get; set; } = string.Empty;
        [Column(TypeName = "timestampz")]
        public DateTime EducationStart { get; set; }
        [Column(TypeName = "timestampz")]
        public DateTime EducationEnd { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string EducationBase { get; set; } = string.Empty;
        public int Course { get; set; }
        public string StudentCard { get; set; } = string.Empty;
    }
}
