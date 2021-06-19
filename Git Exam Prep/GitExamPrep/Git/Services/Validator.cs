using Git.ViewModels;
using GitData;
using System.Collections.Generic;
using System.Linq;

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

            if (this.dbContext.Users.Any(u => u.Username == model.Username))
            {
                validationErrors.Add($"Username '{model.Username}' is already taken.");
            }

            if (this.dbContext.Users.Any(u => u.Email == model.Email))
            {
                validationErrors.Add($"Email '{model.Username}' is already taken.");
            }

            if (model.Username.Length < 5 || model.Username.Length > 20)
            {
                validationErrors.Add($"Username '{model.Username}' is not valid. It must be between 5 and 20 characters long.");
            }
            
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                validationErrors.Add($"Email can not be empty!");
            }

            if (model.Password.Length < 6 || model.Password.Length > 20)
            {
                validationErrors.Add($"Invalid password. It must be between 6 and 20 characters long.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                validationErrors.Add($"Password and its confirmation are different.");
            }

            return validationErrors;
        }
    }
}
