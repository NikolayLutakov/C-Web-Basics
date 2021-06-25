using Git.Data;
using Git.Data.Models;
using Git.Services;
using Git.ViewModels.Commits;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace Git.Controllers
{
    public class CommitsController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;

        public CommitsController(ApplicationDbContext data, IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse All()
        {
            var commits = data.Commits
                .Where(c => c.CreatorId == this.User.Id)
                .Select(c => new CommitsListingViewModel
                {
                    Id = c.Id,
                    Description = c.Description,
                    Repository = c.Repository.Name,
                    CreatedOn = c.CreatedOn.ToLocalTime().ToString("G")
                })
                .ToList();

            return View(commits);
        }

        [Authorize]
        public HttpResponse Create(string id)
        {
            var repo = data.Repositories.Where(r => r.Id == id)
                .Select(r => new CreateCommitGetViewModel
                {
                    RepositoryId = r.Id,
                    RepositoryName = r.Name
                })
                .FirstOrDefault();

            return View(repo);
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Create(CreateCommitViewModel model)
        {
            var errors = validator.ValidateCommit(model);

            if (errors.Any())
            {
                return Error(errors);
            }

            var commit = new Commit
            {
                CreatorId = this.User.Id,
                RepositoryId = model.RepositoryId,
                Description = model.Description
            };

            data.Add(commit);
            data.SaveChanges();

            return Redirect("/Repositories/All");
        }

        public HttpResponse Delete(string id)
        {
            var commit = data.Commits.Find(id);

            if (commit == null || commit.CreatorId != this.User.Id)
            {
                return BadRequest();
            }

            data.Remove(commit);
            data.SaveChanges();

            return Redirect($"/Commits/All");
        }
    }
}
