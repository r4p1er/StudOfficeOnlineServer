namespace StudOfficeOnlineServer.Models.DTOs;

public class AnnouncementData
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
    
    public DateTime Date { get; set; } = new DateTime(0, 0, 0);
    
    public int FacultyId { get; set; } = 0;

    public int GroupId { get; set; } = 0;

    public int Course { get; set; } = 0;
}