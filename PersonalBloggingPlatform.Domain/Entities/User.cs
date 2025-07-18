﻿namespace PersonalBloggingPlatform.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = "User";
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
