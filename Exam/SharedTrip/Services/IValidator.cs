using SharedTrip.ViewModels.Trips;
using SharedTrip.ViewModels.Users;
using System.Collections.Generic;

namespace SharedTrip.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserViewModel model);

        ICollection<string> ValidateTrip(AddTripViewModel model);
    }
}