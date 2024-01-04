using System.ComponentModel.DataAnnotations.Schema;
using Pgvector;

namespace backend.Models.Entity
{
    public class Image
    {
        public Guid Id { get; set; }
        public string? generatedCaption { get; set; }

        [Column(TypeName = "vector(384)")]
        public Vector? CaptionEmbedding { get; set; }
    }
}
