using MongoDB.Bson;

namespace backend.Models.Common
{
    public class BaseFileContentDto
    {
        public ObjectId Id { get; set; }
        public required string FileExtension { get; set; }
    }

}
