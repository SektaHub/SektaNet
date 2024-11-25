namespace backend.Models.Discord
{
    public class Channel
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string? CategoryId { get; set; }
        public string? Category { get; set; }
        public string Name { get; set; }
        public string? Topic { get; set; }

    }
}
