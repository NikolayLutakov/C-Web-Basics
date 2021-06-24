using CarShop.Data;
using CarShop.ViewModels.Cars;
using CarShop.ViewModels.Issues;
using CarShop.ViewModels.Users;
using System;
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

        public ICollection<string> ValidateCar(AddCarViewModel model)
        {
            var validationErrors = new List<string>();

            if (!int.TryParse(model.Year, out int a) || string.IsNullOrEmpty(model.Year))
            {
                validationErrors.Add($"Year {model.Year} is not valid. It must be a valid integer number.");
            }

            if (string.IsNullOrEmpty(model.Model) || model.Model.Length < CarModelMinLength || model.Model.Length > CarModelMaxLength)
            {
                validationErrors.Add($"Invalid car model. It must be between {CarModelMinLength} and {CarModelMinLength} characters long.");
            }

            if (string.IsNullOrEmpty(model.Image) || !Uri.IsWellFormedUriString(model.Image, UriKind.Absolute))
            {
                validationErrors.Add($"Image '{model.Image}' is not valid. It must be a valid URL.");
            }

            if (!Regex.IsMatch(model.PlateNumber, PlateNumberPattern))
            {
                validationErrors.Add($"Plate number '{model.PlateNumber}' is invalid. It must be like 'AA1234AA' or 'AA 1234 AA'");
            }

            return validationErrors;
        }

        public ICollection<string> ValidateIssue(AddIssueViewModel model)
        {
            var validationErrors = new List<string>();

            if (model.Description.Length < IssueDescriptionMinLength || string.IsNullOrEmpty(model.Description))
            {
                validationErrors.Add($"Description must be at least {IssueDescriptionMinLength} characters long.");
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

            if (string.IsNullOrEmpty(model.UserType) || (model.UserType != "Mechanic" && model.UserType != "Client"))
            {
                validationErrors.Add($"User type can only be either a mechanic or client");
            }

            return validationErrors;
        }

    }
}
