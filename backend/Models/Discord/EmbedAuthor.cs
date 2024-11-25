using Microsoft.EntityFrameworkCore;

namespace backend.Models.Discord
{
    [Owned]
    public class EmbedAuthor
    {
        string Name { get; set; }
        string Url { get; set; }
    }
}
