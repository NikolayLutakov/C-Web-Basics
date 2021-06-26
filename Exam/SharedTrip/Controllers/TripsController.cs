using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Data;
using SharedTrip.Models;
using SharedTrip.Services;
using SharedTrip.ViewModels.Trips;
using System;
using System.Globalization;
using System.Linq;

using static SharedTrip.Data.Common.DataConstants;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;

        public TripsController(ApplicationDbContext data, IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse All()
        {
            var trips = data.Trips
                .Select(t => new TripsListingViewModel
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    DepartureTime = t.DepartureTime.ToString("G"),
                    Seats = (t.Seats - t.UserTrips.Where(c => c.TripId == t.Id).Count()).ToString()
                })
                .ToList();

            return View(trips);
        }

        [Authorize]
        public HttpResponse Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddTripViewModel model)
        {
            var errors = validator.ValidateTrip(model);

            if (errors.Any())
            {
                return Error(errors);
            }

            var trip = new Trip
            {
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                DepartureTime = DateTime.ParseExact(model.DepartureTime, TripDepartureTimeFormat, CultureInfo.InvariantCulture),
                ImagePath = model.ImagePath,
                Seats = int.Parse(model.Seats),
                Description = model.Description
            };

            data.Trips.Add(trip);
            data.SaveChanges();

            return Redirect("/Trips/All");
        }

        [Authorize]
        public HttpResponse Details(string tripId)
        {
            var trip = data.Trips.Where(t => t.Id == tripId)
                .Select(t => new TripInfoViewModel
                {
                    Id = t.Id,
                    PictureUrl =t.ImagePath,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    DepartureTime = t.DepartureTime.ToString("G"),
                    Seats = (t.Seats - t.UserTrips.Count(c => c.TripId == t.Id)).ToString(),
                    Description = t.Description,
                    IsJoined = IsJoinedToTrip(tripId, this.User.Id),
                    HasSeats = t.Seats - t.UserTrips.Count(c => c.TripId == t.Id) == 0 
                    ? false : true

                })
                .FirstOrDefault();
            
            if (trip == null)
            {
                return BadRequest();
            }

            return View(trip);
        }

        [Authorize]
        public HttpResponse AddUserToTrip(string tripId)
        {
            var trip = data.Trips.Find(tripId);

            if (trip == null)
            {
                return BadRequest();
            }

            if (IsJoinedToTrip(tripId, this.User.Id))
            {
                return Redirect($"/Trips/Details?tripId={tripId}");
            }

            var userTrip = new UserTrip
            {
                UserId = this.User.Id,
                TripId = tripId
            };

            data.Add(userTrip);
            data.SaveChanges();

            return Redirect("/Trips/All");
        }

        private bool IsJoinedToTrip(string tripId, string userId)
            => data.UserTrips.FirstOrDefault(tr => tr.TripId == tripId && tr.UserId == this.User.Id) != null;
    }
}
