namespace StudOfficeOnlineServer.Models;

public class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public List<Student> Students { get; set; } = new List<Student>();
    public List<Teacher> Teachers { get; set; } = new List<Teacher>();
}