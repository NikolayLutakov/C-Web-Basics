using SharedTrip.Data;
using SharedTrip.ViewModels.Trips;
using SharedTrip.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using static SharedTrip.Data.Common.DataConstants;

namespace SharedTrip.Services
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

        public ICollection<string> ValidateTrip(AddTripViewModel model)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrEmpty(model.StartPoint))
            {
                validationErrors.Add("Start point can't be empty.");
            }

            if (string.IsNullOrEmpty(model.EndPoint))
            {
                validationErrors.Add("End point can't be empty.");
            }

            if (!DateTime.TryParseExact(model.DepartureTime, TripDepartureTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime a))
            {
                validationErrors.Add($"Departure time '{model.DepartureTime}' is not in a valid format. It must be in format '{TripDepartureTimeFormat}'");
            }

            if (!int.TryParse(model.Seats, out int b))
            {
                validationErrors.Add($"Inserted value for seats '{model.Seats}' is invalid. It must be a number.");
            }
            else if (int.Parse(model.Seats) < TripSeatsMinValue || int.Parse(model.Seats) > TripSeatsMaxValue)
            {
                validationErrors.Add($"Number'{model.Seats}' is invalid. It must be number bewtween {TripSeatsMinValue} and {TripSeatsMaxValue}.");
            }

            if (string.IsNullOrEmpty(model.Description) || model.Description.Length > TripDescriptionMaxLength)
            {
                validationErrors.Add($"Inserted description is invalid. It can't be empty and must be up to {TripDescriptionMaxLength} characters long.");
            }

            return validationErrors;
        }
    }
}