using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudOfficeOnlineServer.Models;

public class Announcement
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int FacultyId { get; set; } = 0;

    public int GroupId { get; set; } = 0;

    public int Course { get; set; } = 0;

    public int AdminId { get; set; }
    [Column(TypeName = "timestampz")]
    public DateTime date { get; set; }
}