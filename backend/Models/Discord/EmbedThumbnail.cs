using Microsoft.EntityFrameworkCore;

namespace backend.Models.Discord
{
    [Owned]
    public class EmbedThumbnail
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
