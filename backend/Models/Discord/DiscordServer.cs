

namespace backend.Models.Discord
{
    public class DiscordServer
    {
        public Guid Id { get; set; }
        public Guild Guild { get; set; }
        public Channel Channel { get ; set; }
        public DateTime ExportedAt { get; set; }
        public List<Message> Messages { get; set; }
    }
}
