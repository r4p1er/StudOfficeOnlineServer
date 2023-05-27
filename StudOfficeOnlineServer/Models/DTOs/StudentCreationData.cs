namespace StudOfficeOnlineServer.Models.DTOs;

public class StudentCreationData
{
    public int? GroupId { get; set; }
    
    public int? FacultyId { get; set; }
    
    public EducationForm EducationForm { get; set; }

    public string Citizenship { get; set; } = string.Empty;

    public DateTime EducationStart { get; set; }
    
    public DateTime EducationEnd { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public EducationBase EducationBase { get; set; }

    public List<Subject> Subjects { get; set; } = new();
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    public Role? Role { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string MiddleName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
}