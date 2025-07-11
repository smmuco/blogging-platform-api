﻿using BloggingPlatform.Domain.Entities;

namespace PersonalBloggingPlatform.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public string Author { get; set; }
        public bool IsPublished { get; set; } = false;
        /// Navigation properties
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
