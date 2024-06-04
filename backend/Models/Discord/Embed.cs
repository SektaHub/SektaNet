
using Microsoft.EntityFrameworkCore;

namespace backend.Models.Discord
{
    [Owned]
    public class Embed
    {
        //Added for 
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string? Description { get; set; }
        public DiscordUser? Author { get; set; }
        public EmbedThumbnail? Thumbnail { get; set; }
        public EmbedVideo? EmbedVideo { get; set; }
        //public List<EmbedImage> Images { get; set; }
        //public List<EmbedField> Fields { get; set; }

    }
}
