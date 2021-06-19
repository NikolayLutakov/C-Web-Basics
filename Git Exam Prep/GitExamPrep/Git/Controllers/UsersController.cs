using Git.Services;
using Git.ViewModels;
using GitData;
using GitDataModels;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace Git.Controllers
{
    public class UsersController : Controller
    {
        private readonly IValidator validator;
        private readonly GitDbContext dbContext;
        private readonly IPasswordHasher hasher;

        public UsersController(IValidator validator, GitDbContext dbContext, IPasswordHasher hasher)
        {
            this.dbContext = dbContext;
            this.validator = validator;
            this.hasher = hasher;
        }

        public HttpResponse Login()
        {
            if (this.User.IsAuthenticated)
            {
                return this.Redirect("/Repositories/All");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(LoginUserViewModel model)
        {
            var user = dbContext.Users
                .Where(u => u.Username == model.Username 
                && u.Password == this.hasher.HashPassword(model.Password))
                .Select(u => u.Id)
                .FirstOrDefault();

            if (user == null)
            {
                return Error("Username and password combination is not valid.");
            }

            this.SignIn(user);

            return this.Redirect("/Repositories/All");
        }

        public HttpResponse Register()
        {
            if (this.User.IsAuthenticated)
            {
                return this.Redirect("/Repositories/All");
            }

            return this.View();
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

            dbContext.Users.Add(userToAdd);
            dbContext.SaveChanges();

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
