using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static BattleCards.Data.Common.DataConstants;

namespace BattleCards.Data.Models
{
    public class Card
    {
        public Card()
        {
            this.UserCards = new HashSet<UserCard>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(CardNameMaxLength)]
        public string Name { get; set; }
        
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Keyword { get; set; }

        public int Attack { get; set; }

        public int Health { get; set; }

        [Required]
        [MaxLength(CardDescriptionMaxLength)]
        public string Description { get; set; }


        public IEnumerable<UserCard> UserCards { get; set; }

    }
}
