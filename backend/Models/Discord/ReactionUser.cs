using Microsoft.EntityFrameworkCore;

namespace backend.Models.Discord
{
    [Owned]
    public class ReactionUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Descriminator { get; set; }
        public bool IsBot { get; set; }
        public string AvatarUrl { get; set; }

    }
}
