using System.ComponentModel.DataAnnotations.Schema;

namespace StudOfficeOnlineServer.Models;

public class Faculty
{
    public int Id { get; set; }
    
    [Column(TypeName = "integer")] 
    
    public FacultyNames Name { get; set; }
}