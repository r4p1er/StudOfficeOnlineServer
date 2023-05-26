namespace StudOfficeOnlineServer.Models.DTOs
{
    public class TokenDTO
    {
        public string? Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;
    }
}
