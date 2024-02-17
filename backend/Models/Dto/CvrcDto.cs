using backend.Models.Common;
using backend.Models.Entity;

namespace backend.Models.Dto;

public class CvrcDto : BaseCrudDto
{
    public string message { get; set; }
    public List<Image>? image { get; set; }
}