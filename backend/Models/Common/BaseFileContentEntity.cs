namespace backend.Models.Common
{
    public class BaseFileContentEntity
    {
        public Guid Id { get; set; }
        public required string FileExtension { get; set; }
    }
}
