namespace backend.Models.Dto
{
    public class BlogpostResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Tags { get; set; }
        public string PublisherName { get; set; }
        public DateTime DateCreated { get; set; }
    }

}
