using CarShop.Services;
using CarShop.ViewModels;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace CarShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Login()
        {
            if (this.User.IsAuthenticated)
            {
                return Redirect("/Cars/All");
            }
            return View();
        }

        [HttpPost]
        public HttpResponse Login(UserLoginFormViewModel model)
        {
            var userId = usersService.GetUserId(model.Username, model.Password);

            if (userId == null)
            {
                return Error("Incorrect username or password.");
            }

            this.SignIn(userId);

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
        public HttpResponse Register(UserRegisterFormViewModel model)
        {
            var errors = usersService.Create(model);

            if (errors.Any())
            {
                return Error(errors);
            }

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
