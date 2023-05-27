using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudOfficeOnlineServer.Models.DTOs;

public class StudentDTO
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public GroupDTO Group { get; set; } = new();
    public FacultyDTO Faculty { get; set; } = new();
    public string EducationForm { get; set; } = string.Empty;
    public string Citizenship { get; set; } = string.Empty;
    [Column(TypeName = "timestampz")]
    public DateTime EducationStart { get; set; }
    [Column(TypeName = "timestampz")]
    public DateTime EducationEnd { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string EducationBase { get; set; } = string.Empty;
    public int Course { get; set; }
    public string StudentCard { get; set; } = string.Empty;
}