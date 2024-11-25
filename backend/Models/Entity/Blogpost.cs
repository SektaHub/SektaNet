namespace backend.Models.Entity
{
    public class Blogpost
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Tags { get; set; }
        public List<string> AuthorizedRoles { get; set; } = new List<string>();
        public DateTime DateCreated { get; set; }
        public string? PublisherId { get; set; }
        public ApplicationUser Publisher { get; set; }
    }
}
