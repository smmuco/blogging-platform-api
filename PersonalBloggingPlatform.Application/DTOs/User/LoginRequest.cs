namespace BloggingPlatform.Application.DTOs.User
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
