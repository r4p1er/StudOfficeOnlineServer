namespace StudOfficeOnlineServer.Models;

public class Retake
{
    public int Id { get; set; }
    public string Subject { get; set; }

    public int TeacherId { get; set; }

    public int GroupId { get; set; }

    public DateTime DateTime { get; set; }

    public string Сlassroom { get; set; }
}