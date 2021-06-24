using CarShop.Data;
using CarShop.Data.Models;
using CarShop.Services;
using CarShop.ViewModels.Issues;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace CarShop.Controllers
{
    public class IssuesController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;
        private readonly IUsersService userService;

        public IssuesController(IUsersService userService, ApplicationDbContext data, IValidator validator)
        {
            this.userService = userService;
            this.data = data;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse CarIssues(string carId)
        {
            if (!HasAccess(this.User.Id, carId))
            {
                return Unauthorized();
            }

            var carIssues = data.Cars
                .Where(c => c.Id == carId)
                .Select(c => new IssueCarViewModel
                {
                    Id = c.Id,
                    IsMechanic = userService.IsUserMechanic(this.User.Id),
                    Model = c.Model,
                    Year = c.Year.ToString(),
                    Issues = c.Issues.Select(i => new IssueViewModel
                    {
                        Id = i.Id,
                        Description = i.Description,
                        IsFixed = i.IsFixed == true ? "Yes" : "Not Yet"
                    })
                    .ToList()
                })
                .FirstOrDefault();

            return View(carIssues);
        }

        [Authorize]
        public HttpResponse Fix(string issueId, string CarId)
        {
            if (!userService.IsUserMechanic(this.User.Id))
            {
                return Unauthorized();
            }

            var issue = data.Issues.Where(i => i.Id == issueId && i.CarId == CarId).FirstOrDefault();

            if (issue == null)
            {
                return BadRequest();
            }

            issue.IsFixed = true;

            data.SaveChanges();

            return Redirect($"/Issues/CarIssues?carId={CarId}");
        }

        [Authorize]
        public HttpResponse Delete(string issueId, string CarId)
        {
            var issue = data.Issues.Where(i => i.Id == issueId && i.CarId == CarId).FirstOrDefault();

            if (issue == null)
            {
                return BadRequest();
            }

            data.Remove(issue);
            data.SaveChanges();

            return Redirect($"/Issues/CarIssues?carId={CarId}");
        }

        [Authorize]
        public HttpResponse Add(string CarId)
        {
            return View(model: CarId);
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Add(AddIssueViewModel model)
        {
            if (data.Cars.Find(model.CarId) == null)
            {
                return BadRequest();
            }

            var errors = validator.ValidateIssue(model);

            if (errors.Any())
            {
                return Error(errors);
            }

            var issue = new Issue
            {
                CarId = model.CarId,
                Description = model.Description,
                IsFixed = false
            };

            data.Issues.Add(issue);
            data.SaveChanges();

            return Redirect($"/Issues/CarIssues?carId={model.CarId}");
        }

        private bool HasAccess(string userId, string carId)
        {
            if (data.Cars.FirstOrDefault(c => c.Id == carId).OwnerId == userId || userService.IsUserMechanic(userId))
            {
                return true;
            }

            return false;
        }
    }
}
