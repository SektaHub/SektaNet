namespace backend.Models.Common
{
    public class BaseVideoContentDto : BaseFileContentDto
    {
        public string? AudioTranscription { get; set; }
        public int? Duration { get; set; }
    }
}
