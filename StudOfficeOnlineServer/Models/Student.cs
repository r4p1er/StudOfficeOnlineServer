namespace StudOfficeOnlineServer.Models;

public class Student
{
    public int Id { get; set; }
    
    public int? GroupId { get; set; }
    
    public Group? Group { get; set; }
    
    public int? FacultyId { get; set; }
    
    public Faculty? Faculty { get; set; }

    public string EducationForm { get; set; } = string.Empty;

    public string Citizenship { get; set; } = string.Empty;

    public DateTime EducationStart { get; set; }
    
    public DateTime EducationEnd { get; set; }
    public DateTime OrderDate { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public string EducationBase { get; set; } = string.Empty;
    public int Course { get; set; }
    

    public string StudentCard { get; set; } = string.Empty;

    public int? UserId { get; set; }
    
    public User? User { get; set; }
    
    public List<Subject> Subjects { get; set; } = new();
    
}