namespace backend.Models.Entity
{
    public class Reel
    {
        public Guid Id { get; set; }
        public string? AudioTranscription { get; set; }
        public int? Duration { get; set; }
    }
}
