namespace StudOfficeOnlineServer.Models.DTOs
{
    public class StudentDocumentDTO
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
