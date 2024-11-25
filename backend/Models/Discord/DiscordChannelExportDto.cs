using System.ComponentModel.DataAnnotations;

namespace backend.Models.Discord
{
    public class DiscordChannelExportDto
    {
        public Guid Id { get; set; }
        public Guild Guild { get; set; }
        public Channel Channel { get; set; }
        public string ExportedAt { get; set; }
        public List<Message> Messages { get; set; }
    }
}
