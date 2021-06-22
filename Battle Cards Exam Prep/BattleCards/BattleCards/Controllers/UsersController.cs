using BattleCards.Data.Models;
using BattleCards.Services.PasswordHasher;
using BattleCards.Services.Validator;
using BattleCards.ViewModels.Users;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace BattleCards.Data.Common
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

        #region Login
        public HttpResponse Login()
        {
            if (this.User.IsAuthenticated)
            {
                return Redirect("/Cards/All");
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

            return Redirect("/Cards/All");
        }
        #endregion

        #region Register
        public HttpResponse Register()
        {
            if (this.User.IsAuthenticated)
            {
                return Redirect("/Cards/All");
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
                Password = this.hasher.HashPassword(model.Password)
            };

            data.Users.Add(userToAdd);
            data.SaveChanges();

            return this.Redirect("/Users/Login");
        }
        #endregion

        #region Logout
        [Authorize]
        public HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
        #endregion
    }
}
