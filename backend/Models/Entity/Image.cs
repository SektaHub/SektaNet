using System.ComponentModel.DataAnnotations.Schema;
using backend.Models.Common;
using Pgvector;

namespace backend.Models.Entity
{
    public class Image : BaseFileContentEntity
    {
        //Florence2 caption
        public string? GeneratedCaption { get; set; }

        //Embedding for clip large
        [Column(TypeName = "vector(512)")]
        public Vector? ClipEmbedding { get; set; }
    }
}
