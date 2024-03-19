using backend.Models.Common;
using Pgvector;

namespace backend.Models.Dto
{
    public class ImageDto : BaseFileContentDto
    {
        public string? GeneratedCaption { get; set; }

        public Vector? ClipEmbedding { get; set; }

    }
}
