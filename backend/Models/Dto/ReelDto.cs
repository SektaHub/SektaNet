using backend.Models.Common;

namespace backend.Models.Dto
{
    public class ReelDto : BaseFileContentDto
    {
        public string? AudioTranscription { get; set; }
        public int? Duration { get; set; }
    }
}
