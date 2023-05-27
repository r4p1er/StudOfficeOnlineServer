using System.ComponentModel.DataAnnotations.Schema;

namespace StudOfficeOnlineServer.Models;

public class Faculty
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}