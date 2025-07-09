using System.ComponentModel.DataAnnotations;

namespace PersonalBloggingPlatform.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public bool IsPublished { get; set; } = false;
    }
}
