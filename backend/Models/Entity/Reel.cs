using backend.Models.Common;

namespace backend.Models.Entity
{
    public class Reel : BaseFileContentEntity
    {
        public string? AudioTranscription { get; set; }
        public int? Duration { get; set; }
    }
}
