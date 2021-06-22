using BattleCards.Data;
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

            // Validate email with regex "^[\w-\.]+@([\w-]+\.)+[\w-]{2,}$".
            if (!Regex.IsMatch(model.Email, UserEmailPattern))
            {
                validationErrors.Add($"Invalid email. It must be like 'something@something.something'.");
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
