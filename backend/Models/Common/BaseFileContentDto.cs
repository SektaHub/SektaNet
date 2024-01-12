namespace backend.Models.Common
{
    public class BaseFileContentDto
    {
        public Guid Id { get; set; }
        public required string FileExtension { get; set; }
    }

}
