using BattleCards.Data;
using BattleCards.Services.Validator;
using BattleCards.ViewModels.Cards;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;

        public CardsController(ApplicationDbContext data, IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        #region AllCards
        [Authorize]
        public HttpResponse All()
        {
            var cards = data.Cards
                .Select(c => new CardViewModel
                {
                    Id = c.Id,
                    Title = c.Name,
                    Keyword = c.Keyword,
                    ImageUrl = c.ImageUrl,
                    Description = c.Description,
                    Attack = c.Attack,
                    Health = c.Health
                })
                .ToList();

            return View(cards);
        }
        #endregion
    }
}
