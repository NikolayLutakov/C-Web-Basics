using Git.Data;
using Git.Data.Models;
using Git.Services;
using Git.ViewModels.Repositories;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace Git.Controllers
{
    public class RepositoriesController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;

        public RepositoriesController(ApplicationDbContext data, IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        public HttpResponse All()
        {
            var repositories = data.Repositories
                .Where(r => r.IsPublic || r.OwnerId == this.User.Id)
                .Select(r => new RepositoryListingViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Owner = r.Owner.Username,
                    CreatedOn = r.CreatedOn.ToLocalTime().ToString("G"),
                    CommitsCount = r.Commits.Count()
                })
                .ToList();

            return View(repositories);
        }

        [Authorize]
        public HttpResponse Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Create(CreateRepositoryViewModel model)
        {
            model.CreatorId = this.User.Id;

            var errors = validator.ValidateRepository(model);

            if (errors.Any())
            {
                return Error(errors);
            }

            var repository = new Repository
            {
                Name = model.Name,
                IsPublic = model.RepositoryType == "Public" ? true : false,
                OwnerId = model.CreatorId
            };

            data.Repositories.Add(repository);
            data.SaveChanges();

            return Redirect("/Repositories/All");
        }
    }
}
