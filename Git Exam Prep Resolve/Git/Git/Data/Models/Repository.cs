using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Git.Data.Common.DataConstants;

namespace Git.Data.Models
{
    public class Repository
    {
        public Repository()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
            this.Commits = new HashSet<Commit>();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(RepositoryMaxNameLength)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsPublic { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public IEnumerable<Commit> Commits { get; set; }
    }
}
