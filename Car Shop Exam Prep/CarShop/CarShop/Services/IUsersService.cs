using CarShop.ViewModels;
using System.Collections.Generic;

namespace CarShop.Services
{
    public interface IUsersService
    {
        string GetUserId(string username, string password);

        ICollection<string> Create(UserRegisterFormViewModel model);

        bool IsUserMechanic(string userId);
    }
}
