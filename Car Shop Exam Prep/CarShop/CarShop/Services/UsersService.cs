using CarShop.Data;
using CarShop.Data.Models;
using CarShop.ViewModels;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
namespace CarShop.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext data;
        private readonly IPasswordHasher hasher;
        private readonly IValidator validator;

        public UsersService(ApplicationDbContext data, IPasswordHasher hasher, IValidator validator)
        {
            this.data = data;
            this.hasher = hasher;
            this.validator = validator;
        }

        public ICollection<string> Create(UserRegisterFormViewModel model)
        {
            var errorList = validator.ValidateUser(model);

            if (!errorList.Any())
            {
                var type = model.UserType == "Client" ? false : true;
                var hashedPassword = hasher.HashPassword(model.Password);

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = hashedPassword,
                    IsMechanic = type
                };

                try
                {
                    data.Users.Add(user);
                    data.SaveChanges();
                }
                catch (SqlException e)
                {
                    errorList.Add(e.Message);
                }
            }
          
            return errorList; ;
        }

        public string GetUserId(string username, string password)
            => data.Users.Where(u => u.Username == username && u.Password == hasher.HashPassword(password)).Select(u => u.Id).FirstOrDefault();

        public bool IsUserMechanic(string userId)
            => data.Users.Where(u => u.Id == userId && u.IsMechanic == true).FirstOrDefault() == null ? false : true;


    }
}
