using Git.ViewModels;
using GitData;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using static GitData.Common.DataConstants;

namespace Git.Services
{
    public class Validator : IValidator
    {
        private readonly GitDbContext dbContext;

        public Validator(GitDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ICollection<string> ValidateCommit(CreateCommitViewModel model)
        {
            var validationErrors = new List<string>();

            if (model.Description.Length < 5)
            {
                validationErrors.Add($"Description must be at leasr 5 characters long.");
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                validationErrors.Add($"Description can not be empty.");
            }

            return validationErrors;
        }

        public ICollection<string> ValidateRepository(CreateRepositoryViewModel model)
        {
            var validationErrors = new List<string>();

            if (model.Name.Length < 3 || model.Name.Length > 10)
            {
                validationErrors.Add($"Repository name '{model.Name}' is invalid. It must be between 3 and 10 characters long.");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                validationErrors.Add($"Repository name can not be empty.");
            }

            return validationErrors;
        }

        public ICollection<string> ValidateUser(RegisterUserViewModel model)
        {
            var validationErrors = new List<string>();

            // Validate Username is unique.
            if (this.dbContext.Users.Any(u => u.Username == model.Username))
            {
                validationErrors.Add($"User with username '{model.Username}' already exists.");
            }

            // Validate Email is unique.
            if (this.dbContext.Users.Any(u => u.Email == model.Email))
            {
                validationErrors.Add($"User with email '{model.Email}' already exists.");
            }

            // Validate Username with regex "^[\\w-_]{{{UserMinUsernameLength},{UserMaxUsernameLength}}}$".
            if (!Regex.IsMatch(model.Username, UserUsernamePattern))
            {
                validationErrors.Add($"Invalid username. It must be between {UserMinUsernameLength} and {UserMaxUsernameLength} characters long, and may contains only leters, numbers, underscores and dashes.");
            }

            // Validate Email with regex "^[\w-\.]+@([\w-]+\.)+[\w-]{2,}$".
            if (!Regex.IsMatch(model.Email, UserEmailPattern))
            {
                validationErrors.Add($"Invalid email. It must be like 'something@something.something'.");
            }

            // Validate Password with regex "^[\\w-_]{{{UserMinPasswordLength},{UserMaxPasswordLength}}}$".
            if (!Regex.IsMatch(model.Password, UserPasswordPattern))
            {
                validationErrors.Add($"Invalid password. It must be between {UserMinPasswordLength} and {UserMaxPasswordLength} characters long, and may contains only leters, numbers, underscores and dashes.");
            }

            // Validate Confirm Password.
            if (model.Password != model.ConfirmPassword)
            {
                validationErrors.Add($"Inserted 'Password' and 'Confirm Password' did not match.");
            }

            return validationErrors;
        }

        
    }
}
