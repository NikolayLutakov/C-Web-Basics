using CarShop.ViewModels;
using System.Collections.Generic;

namespace CarShop.Services
{
    public interface IIssueService
    {
        ICollection<string> CreateIssue(CreateIssueViewModel model);

        ICollection<IssueViewModel> GetIssuesForCar(string carId);

        void FixIssue(string issueId);

        void DeleteIssue(string issueId);
    }
}
