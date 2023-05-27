namespace StudOfficeOnlineServer.Models.DTOs;

public class AnnouncementData
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
    
    public int FacultyId { get; set; } = 0;

    public int GroupId { get; set; } = 0;

    public int Course { get; set; } = 0;
}