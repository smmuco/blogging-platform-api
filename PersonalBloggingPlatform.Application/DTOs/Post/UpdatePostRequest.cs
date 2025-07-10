namespace BloggingPlatform.Application.DTOs.Post
{
    public class UpdatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public int CategoryId { get; set; }
    }
}
