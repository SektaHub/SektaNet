using backend.Models.Entity;

namespace backend.Models.Dto;

public class CvrcDto
{
    public Guid Id { get; set; }
    public string message { get; set; }
    public List<Image>? image { get; set; }
}