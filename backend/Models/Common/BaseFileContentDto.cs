using MongoDB.Bson;

namespace backend.Models.Common
{
    public class BaseFileContentDto
    {
        public Guid Id { get; set; }
        public required string ContentId { get; set; }
        public required string Name { get; set; }
        public required string FileExtension { get; set; }
        public string? Tags { get; set; }
        public DateTime DateUploaded { get; set; }
        public bool isPrivate { get; set; }
        public ApplicationUser? Owner { get; set; }
    }

}
