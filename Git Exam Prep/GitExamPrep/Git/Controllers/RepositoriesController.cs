using Git.Services;
using Git.ViewModels;
using GitData;
using GitDataModels;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Linq;

namespace Git.Controllers 
{
    public class RepositoriesController : Controller
    {
        private readonly IValidator validator;
        private readonly GitDbContext dbContext;

        public RepositoriesController(IValidator validator, GitDbContext dbContext)
        {
            this.validator = validator;
            this.dbContext = dbContext;
        }
        public HttpResponse All()
        {
            var repositories = dbContext.Repositories
                .Where(r => r.IsPublic == true)
                .Select(r => new RepositoryViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Owner = r.Owner.Username,
                    CreatedOn = r.CreatedOn.ToLocalTime().ToString(),
                    CommitsCount = r.Commits.Count()
                })
                .ToList();

            return this.View(repositories);
        }

        [Authorize]
        public HttpResponse AllUserRepositories(string id)
        {
            var repositories = dbContext.Repositories
               .Where(r => r.OwnerId == id)
               .Select(r => new RepositoryViewModel
               {
                   Id = r.Id,
                   Name = r.Name,
                   Owner = r.Owner.Username,
                   CreatedOn = r.CreatedOn.ToLocalTime().ToString(),
                   CommitsCount = r.Commits.Count()
               })
               .ToList();

            return this.View(repositories);
        }

        [Authorize]
        public HttpResponse Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Create(CreateRepositoryViewModel model)
        {
            var validationErrors = this.validator.ValidateRepository(model);

            if (validationErrors.Any())
            {
                return Error(validationErrors);
            }

            var repositoryType = model.RepositoryType == "Public" ? true : false;

            var repositoryToAdd = new Repository
            {
                Name = model.Name,
                IsPublic = repositoryType,
                CreatedOn = DateTime.UtcNow,
                OwnerId = this.User.Id
            };

            dbContext.Repositories.Add(repositoryToAdd);
            dbContext.SaveChanges();

            return Redirect("/Repositories/All");
        }

        public HttpResponse Delete(string id)
        {
            var userId = this.User.Id;

            var repositoryToDelete = dbContext.Repositories.Find(id);

            if (repositoryToDelete == null)
            {
                return Error("Invalid repository Id.");
            }

            var commits = dbContext.Commits.Where(c => c.RepositoryId == id).ToList();

            dbContext.RemoveRange(commits);
            

            dbContext.Remove(repositoryToDelete);
            dbContext.SaveChanges();

            return Redirect($"/Repositories/AllUserRepositories?id={userId}");
        }
    }
}
