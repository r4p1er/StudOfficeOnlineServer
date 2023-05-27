namespace StudOfficeOnlineServer.Models.DTOs;

public class StudentCreationData
{
    public int? GroupId { get; set; }
    
    public int? FacultyId { get; set; }
    
    public string EducationForm { get; set; } = string.Empty;

    public string Citizenship { get; set; } = string.Empty;

    public DateTime EducationStart { get; set; }
    
    public DateTime EducationEnd { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public string EducationBase { get; set; } = string.Empty;

    public List<Subject> Subjects { get; set; } = new();
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    
    public string MiddleName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
}