using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static BattleCards.Data.Common.DataConstants;

namespace BattleCards.Data.Models
{
    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserCards = new HashSet<UserCard>();
        }
        
        public string Id { get; set; }

        [Required]
        [MaxLength(UserMaxUsernameLength)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public IEnumerable<UserCard> UserCards { get; set; }
    }
}
