using BattleCards.ViewModels.Users;
using System.Collections.Generic;

namespace BattleCards.Services.Validator
{
    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserViewModel model);

    }
}
