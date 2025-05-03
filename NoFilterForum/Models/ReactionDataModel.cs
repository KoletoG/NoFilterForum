using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace NoFilterForum.Models
{
    public class ReactionDataModel
    {
        [Key]
        public string Id { get; set; }
        public short HighIQ { get; set; }
        public short LowIQ { get; set; }
        public short Like {  get; set; }
        public short Dislike { get; set; }
        public ReactionDataModel() {
         Id= Guid.NewGuid().ToString();
        }
    }
}
