using CarShop.Services;
using CarShop.ViewModels;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace CarShop.Controllers
{
    public class IssuesController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IIssueService issueService;
        private readonly ICarService carService;

        public IssuesController(IUsersService usersService, IIssueService issueService, ICarService carService)
        {
            this.usersService = usersService;
            this.issueService = issueService;
            this.carService = carService;
        }

        [Authorize]
        public HttpResponse CarIssues(string carId)
        {
            var model = new IssueIndexPageViewModel();
            var carModelYear = carService.GetCarModelYear(carId);

            model.CarId = carId;
            model.UserId = this.User.Id;
            model.IsMechanic = usersService.IsUserMechanic(this.User.Id);
            model.Year = carModelYear.Year.ToString();
            model.Model = carModelYear.Model;
            
            model.Issues = issueService.GetIssuesForCar(carId);

            return View(model);
        }

        [Authorize]
        public HttpResponse Add(string carId)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(CreateIssueViewModel model)
        {

            var errors = issueService.CreateIssue(model);

            if (errors.Any())
            {
                return Error(errors);
            }

            return Redirect($"/Issues/CarIssues?carId={model.CarId}");
        }


        [Authorize]
        public HttpResponse Fix(string issueId, string carId)
        {
            if (!usersService.IsUserMechanic(this.User.Id))
            {
                return Unauthorized();
            }

            issueService.FixIssue(issueId);

            return Redirect($"/Issues/CarIssues?carId={carId}");
        }


        [Authorize]
        public HttpResponse Delete(string issueId, string carId)
        {
            issueService.DeleteIssue(issueId);

            return Redirect($"/Issues/CarIssues?carId={carId}");
        }
    }
}
