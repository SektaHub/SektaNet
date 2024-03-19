using backend.Models.Common;

namespace backend.Models.Dto
{
    public class AudioDto : BaseFileContentDto
    {
        public int Duration { get; set; }
    }
}
