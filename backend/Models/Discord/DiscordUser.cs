using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Discord
{
    public class DiscordUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Discriminator { get; set; }
        public string? NickName { get; set; }
        public string? Color { get; set; }
        public bool IsBot { get; set; }
        public string? AvatarUrl { get; set; }

        // Remove these navigation properties from here
        // public ICollection<Message> AuthoredMessages { get; set; }
        // public ICollection<Message> MentioningMessages { get; set; }
    }
}
