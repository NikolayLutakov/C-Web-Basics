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

            if (this.data.Cards.Any(c => c.Name == model.Name))
            {
                validationErrors.Add($"Card with name '{model.Name}' already exists.");
            }

            if (model.Name.Length < CardNameMinLength || model.Name.Length > CardNameMaxLength || string.IsNullOrEmpty(model.Name))
            {
                validationErrors.Add($"Card name must be between {CardNameMinLength} and {CardNameMaxLength} characters long.");
            }

            if (string.IsNullOrEmpty(model.Image))
            {
                validationErrors.Add($"ImageUrl can't be empty.");
            }

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

            if (model.Description.Length > CardDescriptionMaxLength)
            {
                validationErrors.Add($"Description must be maximum {CardDescriptionMaxLength} characters long.");
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                validationErrors.Add($"Description can't be empty.");
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
