using CarShop.Data;
using CarShop.Data.Models;
using CarShop.ViewModels;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace CarShop.Services
{
    public class IssueService : IIssueService
    {

        private readonly ApplicationDbContext data;
        private readonly IValidator validator;

        public IssueService(ApplicationDbContext data, IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        public ICollection<string> CreateIssue(CreateIssueViewModel model)
        {
            var errors = validator.ValidateIssue(model);

            if (!errors.Any())
            {
                var issue = new Issue
                {
                    CarId = model.CarId,
                    Description = model.Description,
                    IsFixed = false
                };

                try
                {
                    data.Issues.Add(issue);
                    data.SaveChanges();
                }
                catch (SqlException e)
                {
                    errors.Add(e.Message);
                }
            }

            return errors;
        }

        public void DeleteIssue(string issueId)
        {
            var item = data.Issues.Find(issueId);

            data.Remove(item);
            data.SaveChanges();
        }

        public void FixIssue(string issueId)
        {
            var item = data.Issues.Find(issueId);

            item.IsFixed = true;

            data.Update(item);
            data.SaveChanges();
        }

        public ICollection<IssueViewModel> GetIssuesForCar(string carId)
        {
            var issues = data.Issues
                .Where(c => c.CarId == carId)
                .Select(i => new IssueViewModel
                {
                    Id = i.Id,
                    Description = i.Description,
                    IsFixed = i.IsFixed ? "Yes" : "Not Yet"
                })
                .ToList();

            return issues;
        }

    }
}
