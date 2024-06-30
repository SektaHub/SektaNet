

namespace backend.Models.Discord
{
    //Refers to the export, needs rename
    public class DiscordServer
    {
        public Guid Id { get; set; }
        public Guild Guild { get; set; }
        public Channel Channel { get ; set; }
        public string ExportedAt { get; set; }
        public List<Message> Messages { get; set; }
    }
}
