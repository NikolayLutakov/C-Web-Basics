using CarShop.Data;
using CarShop.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using static CarShop.Data.Common.DataConstants;

namespace CarShop.Services
{

    public class Validator : IValidator
    {
        private readonly ApplicationDbContext data;

        public Validator(ApplicationDbContext dbContext)
        {
            this.data = dbContext;
        }

        public ICollection<string> ValidateCar(CreateCarViewModel model)
        {
            var validationErrors = new List<string>();

            if (model.Model.Length < CarModelMinLength || model.Model.Length > MaxLengthTwenty)
            {
                validationErrors.Add($"Model '{model.Model}' is not valid. It must be between {CarModelMinLength} and {MaxLengthTwenty} characters long.");
            }

            if (string.IsNullOrWhiteSpace(model.Year.ToString()))
            {
                validationErrors.Add($"Year can not be empty!");
            }

            if (string.IsNullOrWhiteSpace(model.Image))
            {
                validationErrors.Add($"Image Url can not be empty!");
            }

            if (!Regex.IsMatch(model.PlateNumber, CarPlateNumberPattern))
            {
                validationErrors.Add($"Invalid plate number. It must be in the format 'AA 1111 AA'.");
            }

            return validationErrors;
        }

        public ICollection<string> ValidateIssue(CreateIssueViewModel model)
        {
            var validationErrors = new List<string>();

            if (model.Description.Length < IssueDescriptionMinLength)
            {
                validationErrors.Add($"Description length must be more than {IssueDescriptionMinLength} characters long.");
            }

            return validationErrors;
        }

        public ICollection<string> ValidateUser(UserRegisterFormViewModel model)
        {
            var validationErrors = new List<string>();

            if (this.data.Users.Any(u => u.Username == model.Username))
            {
                validationErrors.Add($"Username '{model.Username}' is already taken.");
            }

            if (this.data.Users.Any(u => u.Email == model.Email))
            {
                validationErrors.Add($"Email '{model.Email}' is already taken.");
            }

            if (model.Username.Length < UserMinUsernameLength || model.Username.Length > MaxLengthTwenty)
            {
                validationErrors.Add($"Username '{model.Username}' is not valid. It must be between {UserMinUsernameLength} and {MaxLengthTwenty} characters long.");
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                validationErrors.Add($"Email can not be empty!");
            }

            if (model.Password.Length < UserMinPasswordLength || model.Password.Length > MaxLengthTwenty)
            {
                validationErrors.Add($"Invalid password. It must be between {UserMinPasswordLength} and {MaxLengthTwenty} characters long.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                validationErrors.Add($"Password and its confirmation are different.");
            }

            return validationErrors;
        }
    }
}
