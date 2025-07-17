namespace BloggingPlatform.Application.DTOs.Post
{
    public class CreatePostRequest
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
    }
}
