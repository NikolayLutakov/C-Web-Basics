using CarShop.Data;
using CarShop.Data.Models;
using CarShop.Services;
using CarShop.ViewModels.Users;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace CarShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;
        private readonly IPasswordHasher hasher;

        public UsersController(IValidator validator, ApplicationDbContext data, IPasswordHasher hasher)
        {
            this.data = data;
            this.validator = validator;
            this.hasher = hasher;
        }

        public HttpResponse Login()
        {
            if (this.User.IsAuthenticated)
            {
                return this.Redirect("/Cars/All");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(LoginUserViewModel model)
        {
            var user = data.Users
                .Where(u => u.Username == model.Username
                && u.Password == this.hasher.HashPassword(model.Password))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (user == null)
            {
                return Error("Incorrect username or pasword.");
            }

            this.SignIn(user);

            return Redirect("/Cars/All");
        }

        public HttpResponse Register()
        {
            if (this.User.IsAuthenticated)
            {
                return Redirect("/Cars/All");
            }

            return View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterUserViewModel model)
        {
            var validationErrors = this.validator.ValidateUser(model);

            if (validationErrors.Any())
            {
                return Error(validationErrors);
            }

            var userToAdd = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = this.hasher.HashPassword(model.Password),
                IsMechanic = model.UserType == "Mechanic"
            };

            data.Users.Add(userToAdd);
            data.SaveChanges();

            return Redirect("/Users/Login");
        }

        [Authorize]
        public HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/");
        }

    }
}
