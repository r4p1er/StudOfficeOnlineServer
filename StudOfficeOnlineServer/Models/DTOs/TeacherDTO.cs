namespace StudOfficeOnlineServer.Models.DTOs
{
    public class TeacherDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<SubjectDTO> Subjects { get; set; } = new();
    }
}
