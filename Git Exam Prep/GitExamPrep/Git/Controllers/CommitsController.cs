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
    public class CommitsController : Controller
    {
        private readonly IValidator validator;
        private readonly GitDbContext dbContext;

        public CommitsController(IValidator validator, GitDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse Create(string id)
        {
            var repository = dbContext.Repositories
                .Where(r => r.Id == id)
                .Select(r => new RepositoryViewModel
                {
                    Id = r.Id,
                    Name = r.Name
                    
                })
                .FirstOrDefault();


            return View(repository);
        }


        [Authorize]
        [HttpPost]
        public HttpResponse Create(CreateCommitViewModel model)
        {
            var validationErrors = this.validator.ValidateCommit(model);

            if (validationErrors.Any())
            {
                return Error(validationErrors);
            }


            var commitToAdd = new Commit
            {
                CreatorId = this.User.Id,
                CreatedOn = DateTime.UtcNow,
                Description = model.Description,
                RepositoryId = model.Id
            };

            dbContext.Commits.Add(commitToAdd);
            dbContext.SaveChanges();

            return Redirect("/Repositories/All");
        }

        [Authorize]
        public HttpResponse All()
        {
            var commits = dbContext.Commits
                .Where(c => c.CreatorId == User.Id)
                .Select(c => new CommitViewModel
                {
                    Id = c.Id,
                    Repository = c.Repository.Name,
                    Description = c.Description,
                    CreatedOn = c.CreatedOn.ToLocalTime().ToString()
                })
                .ToList();

            return View(commits);
        }

        [Authorize]
        public HttpResponse Delete(string id)
        {
            var commitTodelete = dbContext.Commits.Find(id);

            if (commitTodelete == null)
            {
                return Error("Invalid commit Id");
            }

            dbContext.Remove(commitTodelete);
            dbContext.SaveChanges();

            return Redirect("/Commits/All");
        }
    }
}
