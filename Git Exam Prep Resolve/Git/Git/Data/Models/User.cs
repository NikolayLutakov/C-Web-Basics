using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Git.Data.Common.DataConstants;

namespace Git.Data.Models
{
    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Commits = new HashSet<Commit>();
            this.Repositories = new HashSet<Repository>();
        }

        public string Id { get; set; }
        
        [Required]
        [MaxLength(UserMaxUsernameLength)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public IEnumerable<Repository> Repositories { get; set; }

        public IEnumerable<Commit> Commits { get; set; }
    }
}