namespace StudOfficeOnlineServer.Models;

public class Student
{
    public int Id { get; set; }
    
    public int GroupId { get; set; }
    
    public int FacultyId { get; set; }

    public EducationForm EducationForm { get; set; }

    public string Citizenship { get; set; }

    public DateTime EducationStart { get; set; }
    
    public DateTime EducationEnd { get; set; }

    public string OrderNumber { get; set; }

    public EducationBase EducationBase { get; set; }

    public int UserId { get; set; }
}