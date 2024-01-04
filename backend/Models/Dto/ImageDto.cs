using Pgvector;

namespace backend.Models.Dto
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public string? generatedCaption { get; set; }

        public Vector? CaptionEmbedding { get; set; }
    }
}
