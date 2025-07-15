namespace BloggingPlatform.Application.DTOs.User
{
    public class UpdateUserRequest
    {
        public int UserId { get; set; }
        public string? NewPassword { get; set; }
        public string? NewEmail { get; set; }
        public string? NewUsername { get; set; }
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
