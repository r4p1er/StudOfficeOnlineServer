namespace StudOfficeOnlineServer.Models.DTOs
{
    public class ConsultationTicketDTO
    {
        public string Time { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Course { get; set; }
        public string Group { get; set; } = string.Empty;
    }
}
