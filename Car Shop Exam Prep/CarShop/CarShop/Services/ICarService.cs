using CarShop.ViewModels;
using System.Collections.Generic;

namespace CarShop.Services
{
    public interface ICarService
    {
        ICollection<string> CreateCar(CreateCarViewModel model);

        ICollection<CarViewModel> GetAllCarsForUser(string userId);

        ICollection<CarViewModel> GetAllCarsWithIssues();

        CarModelYearViewModel GetCarModelYear(string id);
    }
}
