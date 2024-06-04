using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Discord
{
    public class DiscordUser
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Descriminator { get; set; }
        public string? NickName { get; set; }
        public string? Color { get; set; }
        public bool IsBot { get; set; }
        [NotMapped]
        public List<DiscordRole>? Roles { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
