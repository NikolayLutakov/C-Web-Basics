using System;
using System.ComponentModel.DataAnnotations;
using static Git.Data.Common.DataConstants;

namespace Git.Data.Models
{
    public class Commit
    {
        public Commit()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
        }
        public string Id { get; set; }

        [Required]
        public string Description { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        [Required]
        public string CreatorId { get; set; }

        public User Creator { get; set; }

        [Required]
        public string RepositoryId { get; set; }

        public Repository Repository { get; set; }
    }
}
