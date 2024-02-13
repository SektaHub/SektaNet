namespace backend.Models.Entity;

public class Cvrc
{
    public Guid Id { get; set; }
    public string message { get; set; }
    public List<Image>? image { get; set; }

    // TODO like feature when accounts exist
}