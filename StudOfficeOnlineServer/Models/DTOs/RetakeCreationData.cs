namespace StudOfficeOnlineServer.Models.DTOs;

public class RetakeCreationData
{
    public string Subject { get; set; }

    public int TeacherId { get; set; }

    public int GroupId { get; set; }

    public DateTime DateTime { get; set; }

    public string Сlassroom { get; set; }
}