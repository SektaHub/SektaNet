namespace backend.Models.Dto
{
    public class BlogpostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Tags { get; set; }
        public List<string> AuthorizedRoles { get; set; } = new List<string>();
    }

}
