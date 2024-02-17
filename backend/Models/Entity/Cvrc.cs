using backend.Models.Common;

namespace backend.Models.Entity;

public class Cvrc : BaseCrudEntity
{
    public string message { get; set; }
    public List<Image>? image { get; set; }

    // TODO like feature when accounts exist
}