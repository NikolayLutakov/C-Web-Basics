using CarShop.ViewModels;
using System.Collections.Generic;

namespace CarShop.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateUser(UserRegisterFormViewModel model);

        ICollection<string> ValidateCar(CreateCarViewModel model);

        ICollection<string> ValidateIssue(CreateIssueViewModel model);

    }
}
