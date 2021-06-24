using CarShop.Data;
using CarShop.Data.Models;
using CarShop.Services;
using CarShop.ViewModels.Cars;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace CarShop.Controllers
{
    public class CarsController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;
        private readonly IUsersService userService;

        public CarsController(IValidator validator, ApplicationDbContext data, IUsersService userService)
        {
            this.data = data;
            this.validator = validator;
            this.userService = userService;
        }

        [Authorize]
        public HttpResponse All()
        {
            var carsQuery = this.data
                .Cars
                .AsQueryable();

            if (userService
                .IsUserMechanic(this.User.Id))
            {
                carsQuery = carsQuery
                    .Where(x => x.Issues.Any(i => i.IsFixed == false));
            }
            else
            {
                carsQuery = carsQuery
                    .Where(x => x.OwnerId == this.User.Id);
            }

            var cars = carsQuery
                .Select(x => new CarViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    ImageUrl = x.PictureUrl,
                    PlateNumber = x.PlateNumber,
                    Year = x.Year,
                    FixedIssues = x.Issues
                        .Where(i => i.IsFixed == true)
                        .Count(),
                    RemainingIssues = x.Issues
                        .Where(i => i.IsFixed == false)
                        .Count()
                })
                .ToList();

            return View(cars);
        }

        [Authorize]
        public HttpResponse Add()
        {
            if (userService.IsUserMechanic(this.User.Id))
            {
                return Redirect("/Cars/All");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Add(AddCarViewModel model)
        {
            var errors = this.validator.ValidateCar(model);

            if (errors.Any())
            {
                return Error(errors);
            }

            var car = new Car
            {
                Model = model.Model,
                PictureUrl = model.Image,
                PlateNumber = model.PlateNumber,
                Year = int.Parse(model.Year),
                OwnerId = this.User.Id
            };

            data.Cars.Add(car);
            data.SaveChanges();

            return Redirect("/Cars/All");
        }
    }
}
