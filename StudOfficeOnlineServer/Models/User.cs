namespace StudOfficeOnlineServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int Role { get; set; }
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
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
