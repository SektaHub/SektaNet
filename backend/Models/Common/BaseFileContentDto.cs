using MongoDB.Bson;

namespace backend.Models.Common
{
    public class BaseFileContentDto
    {
        public string Id { get; set; }
        public required string FileExtension { get; set; }
    }

}
