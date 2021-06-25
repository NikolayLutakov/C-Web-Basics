using Git.Data;
using Git.ViewModels.Commits;
using Git.ViewModels.Repositories;
using Git.ViewModels.Users;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static Git.Data.Common.DataConstants;

namespace Git.Services
{
    public class Validator : IValidator
    {
        private readonly ApplicationDbContext data;

        public Validator(ApplicationDbContext dbContext)
        {
            this.data = dbContext;
        }

        public ICollection<string> ValidateCommit(CreateCommitViewModel model)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrEmpty(model.Description) || model.Description.Length < CommitMinDescriptionLength)
            {
                validationErrors.Add($"Invalid commit description. It must be at least  {CommitMinDescriptionLength} characters long.");
            }

            return validationErrors;
        }

        public ICollection<string> ValidateRepository(CreateRepositoryViewModel model)
        {
            var validationErrors = new List<string>();

            if (this.data.Repositories.Any(r => r.Name == model.Name && r.OwnerId == model.CreatorId))
            {
                validationErrors.Add($"Repository with name '{model.Name}' already exists for this user.");
            }

            if (string.IsNullOrEmpty(model.Name) || model.Name.Length < RepositoryMinNameLength || model.Name.Length > RepositoryMaxNameLength)
            {
                validationErrors.Add($"Invalid repository name. It must be between {RepositoryMinNameLength} and {RepositoryMaxNameLength} characters long.");
            }

            if (string.IsNullOrEmpty(model.RepositoryType) || (model.RepositoryType != "Public" && model.RepositoryType != "Private"))
            {
                validationErrors.Add("Invalid repository type. It must be either a public or a private.");
            }

            return validationErrors;
        }

        public ICollection<string> ValidateUser(RegisterUserViewModel model)
        {
            var validationErrors = new List<string>();

            if (this.data.Users.Any(u => u.Username == model.Username))
            {
                validationErrors.Add($"User with username '{model.Username}' already exists.");
            }

            if (this.data.Users.Any(u => u.Email == model.Email))
            {
                validationErrors.Add($"User with email '{model.Email}' already exists.");
            }

            if (model.Username.Length < UserMinUsernameLength || model.Username.Length > UserMaxUsernameLength || string.IsNullOrEmpty(model.Username))
            {
                validationErrors.Add($"Invalid username. It must be between {UserMinUsernameLength} and {UserMaxUsernameLength} characters long.");
            }
            
            if (!string.IsNullOrEmpty(model.Username) && model.Username.Any(x => x == ' '))
            {
                validationErrors.Add("Username cannot contain witespaces");
            }

            if (!Regex.IsMatch(model.Email, UserEmailPattern))
            {
               validationErrors.Add($"Email {model.Email} is not valid e-mail address.");
            }

            if (model.Password.Length < UserMinPasswordLength || model.Password.Length > UserMaxPasswordLength || string.IsNullOrEmpty(model.Password))
            {
                validationErrors.Add($"Invalid password. It must be between {UserMinPasswordLength} and {UserMaxPasswordLength} characters long.");
            }

            if (!string.IsNullOrEmpty(model.Password) && model.Password.Any(x => x == ' '))
            {
                validationErrors.Add("Password cannot contain witespaces");
            }

            if (model.Password != model.ConfirmPassword)
            {
                validationErrors.Add($"Inserted 'Password' and 'Confirm Password' did not match.");
            }

            return validationErrors;
        }

    }
}