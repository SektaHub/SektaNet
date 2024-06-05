
using Microsoft.EntityFrameworkCore;

namespace backend.Models.Discord
{
    [Owned]
    public class Emoji
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsAnimated { get; set; }
        public string ImageUrl { get; set; }
    }
}
