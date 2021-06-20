using CarShop.Services;
using CarShop.ViewModels;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Collections.Generic;
using System.Linq;

namespace CarShop.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService carService;
        private readonly IUsersService usersService;

        public CarsController(ICarService carService, IUsersService usersService)
        {
            this.carService = carService;
            this.usersService = usersService;
        }

        [Authorize]
        public HttpResponse All()
        {
            ICollection<CarViewModel> model;

            if (usersService.IsUserMechanic(this.User.Id))
            {
                model = carService.GetAllCarsWithIssues();
            }
            else
            {
                model = carService.GetAllCarsForUser(this.User.Id);
            }

            return View(model);
        }

        [Authorize]
        public HttpResponse Add()
        {
            if (usersService.IsUserMechanic(this.User.Id))
            {
                return Redirect("/Cars/All");
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(CreateCarViewModel model)
        {
            if (usersService.IsUserMechanic(this.User.Id))
            {
                return Redirect("/Cars/All");
            }

            model.OwnerId = this.User.Id;

            var errors = carService.CreateCar(model);

            if (errors.Any())
            {
                return Error(errors);
            }

            return Redirect("/Cars/All");
        }
    }
}
