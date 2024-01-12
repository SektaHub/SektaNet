using backend.Models.Common;
using Pgvector;

namespace backend.Models.Dto
{
    public class ImageDto : BaseFileContentDto
    {
        public string? generatedCaption { get; set; }

        public Vector? CaptionEmbedding { get; set; }

    }
}
