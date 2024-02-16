using MongoDB.Bson;

namespace backend.Models.Common
{
    public class BaseFileContentEntity
    {
        public ObjectId Id { get; set; }
        public required string FileExtension { get; set; }
    }
}
