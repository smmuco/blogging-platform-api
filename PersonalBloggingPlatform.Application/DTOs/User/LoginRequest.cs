namespace BloggingPlatform.Application.DTOs.User
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
        public string ReturnUrl { get; set;  } = "/";
    }
}
