using backend.Models.Entity;

namespace backend.Models.Common
{
    public class BaseVideoContentEntity : BaseFileContentEntity
    {
        public string? AudioTranscription { get; set; }
        public int Duration { get; set; }
        public string? ThumbnailId { get; set; }
        public Thumbnail? Thumbnail { get; set; }

    }
    
}
