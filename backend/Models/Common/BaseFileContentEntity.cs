using MongoDB.Bson;

namespace backend.Models.Common
{
    public class BaseFileContentEntity
    {
        public required Guid Id { get; set; }
        //Id of the file content in the mongoDb repository
        public required string ContentId { get; set; }
        public required string Name { get; set; }
        public required string FileExtension { get; set; }
        public string? Tags { get; set; }
        public DateTime DateUploaded { get; set; }
        public string? OwnerId { get; set; }
        public ApplicationUser? Owner { get; set; }
        public List<string> AuthorizedRoles { get; set; } = new List<string>();
    }
}
