using Microsoft.EntityFrameworkCore;

namespace backend.Models.Discord
{
    public class Message
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime? TimeStampEdited { get; set; }
        public DateTime? CallEndedTimeStamp { get; set; }
        public bool IsPinned { get; set; }
        public string Content { get; set; }

        public string? AuthorId { get; set; }
        public DiscordUser? Author { get; set; }

        public List<Attachment> Attachments { get; set; }
        public List<Embed> Embeds { get; set; }
        public List<Reaction> Reactions { get; set; }
        public List<DiscordUser> Mentions { get; set; }
    }
}
