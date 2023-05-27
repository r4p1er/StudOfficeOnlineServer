using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudOfficeOnlineServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? StudentId { get; set; }
        public Student? Student { get; set; }
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public int? AdminId { get; set; }
        public Admin? Admin { get; set; }
        public string? RefreshToken { get; set; }
        [Column(TypeName = "timestampz")]
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
