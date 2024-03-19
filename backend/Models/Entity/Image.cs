using System.ComponentModel.DataAnnotations.Schema;
using backend.Models.Common;
using Pgvector;

namespace backend.Models.Entity
{
    public class Image : BaseFileContentEntity
    {
        public string? GeneratedCaption { get; set; }

        [Column(TypeName = "vector(768)")]
        public Vector? ClipEmbedding { get; set; }
    }
}
