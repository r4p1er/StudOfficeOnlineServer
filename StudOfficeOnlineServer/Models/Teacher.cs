using StudOfficeOnlineServer.Models.DTOs;

namespace StudOfficeOnlineServer.Models;

public class Teacher
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public List<Subject> Subjects { get; set; } = new();
}