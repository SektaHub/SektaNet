using MongoDB.Bson;

namespace backend.Models.Common
{
    public class BaseFileContentEntity
    {
        public string Id { get; set; }
        public required string FileExtension { get; set; }
        public string? Tags { get; set; }
    }
}
