using MongoDB.Bson;

namespace backend.Models.Common
{
    public class BaseFileContentDto
    {
        public Guid Id { get; set; }
        public required string ContentId { get; set; }
        public required string Name { get; set; }
        public required string FileExtension { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public DateTime DateUploaded { get; set; }
        public bool isPrivate { get; set; }
        public ApplicationUser? Owner { get; set; }
        public List<string> AuthorizedRoles { get; set; } = new List<string>();
        public string? OriginalSource { get; set; }
    }

}
