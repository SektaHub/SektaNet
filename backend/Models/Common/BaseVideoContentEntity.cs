namespace backend.Models.Common
{
    public class BaseVideoContentEntity : BaseFileContentEntity
    {
        public string? AudioTranscription { get; set; }
        public int? Duration { get; set; }
    }
    
}
