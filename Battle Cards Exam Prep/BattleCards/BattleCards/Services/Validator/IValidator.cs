using System.Collections.Generic;
using BattleCards.ViewModels.Cards;
using BattleCards.ViewModels.Users;

namespace BattleCards.Services.Validator
{
    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserViewModel model);

        ICollection<string> ValidateCard(AddCardViewModel model);

    }
}
