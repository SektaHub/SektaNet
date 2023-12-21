namespace backend.Models.Dto
{
    public class ReelDto
    {
        public Guid Id { get; set; }
        public string? AudioTranscription { get; set; }
        public int? Duration { get; set; }
    }
}
