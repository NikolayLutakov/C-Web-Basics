using BattleCards.Data;
using BattleCards.Data.Models;
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

        #region AddCard
        [Authorize]
        public HttpResponse Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddCardViewModel model)
        {
            var errors = validator.ValidateCard(model);

            if (errors.Any())
            {
                return Error(errors);
            }

            var card = new Card
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.Image,
                Keyword = model.Keyword,
                Attack = int.Parse(model.Attack),
                Health = int.Parse(model.Health),
            };

            var userCard = new UserCard
            {
                UserId = this.User.Id,
                Card = card
            };

            data.Cards.Add(card);
            data.UserCards.Add(userCard);

            data.SaveChanges();

            return Redirect("/Cards/All");
        }
        #endregion

        #region Collection
        [Authorize]
        public HttpResponse Collection()
        {
            var cards = data.Cards
                .Where(x => x.UserCards.Any(uc => uc.UserId == this.User.Id))
                .Select(x => new CardViewModel
                {
                    Id = x.Id,
                    Title = x.Name,
                    Description = x.Description,
                    Keyword = x.Keyword,
                    ImageUrl = x.ImageUrl,
                    Attack = x.Attack,
                    Health = x.Health
                })
                .ToList();

            return View(cards);
        }

        [Authorize]
        public HttpResponse AddToCollection(int cardId)
        {
            var card = data.Cards.Find(cardId);

            if (card == null)
            {
                return Error("Invalid card!");
            }

            var collection = data.Cards.Where(x => x.UserCards.Any(c => c.UserId == this.User.Id));

            if (collection.Contains(card))
            {
                return Redirect("/Cards/All");
            }

            var userCard = new UserCard
            {
                Card = card,
                UserId = this.User.Id
            };

            data.UserCards.Add(userCard);
            data.SaveChanges();

            return Redirect("/Cards/All");
        }

        [Authorize]
        public HttpResponse RemoveFromCollection(int cardId)
        {
            var userCard = data.UserCards
                .Where(x => x.CardId == cardId && x.UserId == this.User.Id)
                .FirstOrDefault();

            if (userCard == null)
            {
                return Error("Invalid card!");
            }

            data.UserCards.Remove(userCard);
            data.SaveChanges();

            return Redirect("/Cards/Collection");
        }
        #endregion
    }
}
