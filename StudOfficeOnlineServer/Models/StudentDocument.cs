namespace StudOfficeOnlineServer.Models
{
    public class StudentDocument
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public Student? Student { get; set; }
        public string Path { get; set; } = string.Empty;
    }
}
