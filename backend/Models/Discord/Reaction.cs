using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Discord
{
    [Owned]
    public class Reaction
    {
        public Emoji Emoji { get; set; }
        public int Count { get; set; }
        [NotMapped]
        public List<ReactionUser> Users { get; set; }
    }
}
