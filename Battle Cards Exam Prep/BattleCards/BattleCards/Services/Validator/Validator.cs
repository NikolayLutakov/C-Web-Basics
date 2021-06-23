using BattleCards.Data;
using BattleCards.ViewModels.Cards;
using BattleCards.ViewModels.Users;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static BattleCards.Data.Common.DataConstants;

namespace BattleCards.Services.Validator
{
    public class Validator : IValidator
    {
        private readonly ApplicationDbContext data;

        public Validator(ApplicationDbContext dbContext)
        {
            this.data = dbContext;
        }

        public ICollection<string> ValidateCard(AddCardViewModel model)
        {
            var validationErrors = new List<string>();

            // Validate name if name is unique.
            if (this.data.Cards.Any(c => c.Name == model.Name))
            {
                validationErrors.Add($"Card with name '{model.Name}' already exists.");
            }

            // Validate name min and max length.
            if (model.Name.Length < CardNameMinLength || model.Name.Length > CardNameMaxLength || string.IsNullOrEmpty(model.Name))
            {
                validationErrors.Add($"Card name must be between {CardNameMinLength} and {CardNameMaxLength} characters long.");
            }

            // Validate ImageUrl
            if (string.IsNullOrEmpty(model.Image))
            {
                validationErrors.Add($"ImageUrl can't be empty.");
            }

            // Validate attack
            if (string.IsNullOrEmpty(model.Attack))
            {
                validationErrors.Add($"Attack can't be empty");                
            }
            else if (!int.TryParse(model.Attack, out int a))
            {
                validationErrors.Add($"Invalid value for Attack. Value is either too large for integer or can't be converted.");
            }
            else if (int.Parse(model.Attack) < CardStatsMinValue)
            {
                validationErrors.Add($"Attack can't be negative");
            }
            
            // Validate Health
            if (string.IsNullOrEmpty(model.Health))
            {
                validationErrors.Add($"Health can't be empty");  
            }
            else if (!int.TryParse(model.Health, out int a))
            {
                validationErrors.Add($"Invalid value for Health. Value is either too large for integer or can't be converted.");
            }
            else if (int.Parse(model.Health) < CardStatsMinValue)
            {
                validationErrors.Add($"Health can't be negative");
            }

            // Validate Description max length
            if (model.Description.Length > CardDescriptionMaxLength)
            {
                validationErrors.Add($"Description must be maximum {CardDescriptionMaxLength} characters long.");
            }

            // Validate Description is not null
            if (string.IsNullOrEmpty(model.Description))
            {
                validationErrors.Add($"Description can't be empty.");
            }

            return validationErrors;
        }

        public ICollection<string> ValidateUser(RegisterUserViewModel model)
        {
            var validationErrors = new List<string>();

            // Validate if username is unique.
            if (this.data.Users.Any(u => u.Username == model.Username))
            {
                validationErrors.Add($"User with username '{model.Username}' already exists.");
            }

            // Validate if email is unique.
            if (this.data.Users.Any(u => u.Email == model.Email))
            {
                validationErrors.Add($"User with email '{model.Email}' already exists.");
            }

            // Validate username with regex "^[\\w-_]{{{UserMinUsernameLength},{UserMaxUsernameLength}}}$".
            if (!Regex.IsMatch(model.Username, UserUsernamePattern))
            {
                validationErrors.Add($"Invalid username. It must be between {UserMinUsernameLength} and {UserMaxUsernameLength} characters long, and may contains only leters, numbers, underscores and dashes.");
            }

            // Validate email with regex "^[\w-\.]+@([\w-]+\.)$".
            if (!Regex.IsMatch(model.Email, UserEmailPattern))
            {
                validationErrors.Add($"Invalid email. It must be like 'something@something'.");
            }

            // Validate password with regex "^[\\w-_]{{{UserMinPasswordLength},{UserMaxPasswordLength}}}$".
            if (!Regex.IsMatch(model.Password, UserPasswordPattern))
            {
                validationErrors.Add($"Invalid password. It must be between {UserMinPasswordLength} and {UserMaxPasswordLength} characters long, and may contains only leters, numbers, underscores and dashes.");
            }

            // Validate password confirmation.
            if (model.Password != model.ConfirmPassword)
            {
                validationErrors.Add($"Inserted 'Password' and 'Confirm Password' did not match.");
            }

            return validationErrors;
        }

    }
}
