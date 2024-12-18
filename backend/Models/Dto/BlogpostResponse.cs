namespace backend.Models.Dto
{
    public class BlogpostResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid? ThumbnailId { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string PublisherName { get; set; }
        public DateTime DateCreated { get; set; }
    }

}
