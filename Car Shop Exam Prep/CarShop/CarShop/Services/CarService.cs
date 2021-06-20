using CarShop.Data;
using CarShop.Data.Models;
using CarShop.ViewModels;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace CarShop.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext data;
        private readonly IValidator validator;

        public CarService(ApplicationDbContext data, IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        public ICollection<string> CreateCar(CreateCarViewModel model)
        {
            var errors = validator.ValidateCar(model);

            if (!errors.Any())
            {
                var car = new Car
                {
                    Model = model.Model,
                    PictureUrl = model.Image,
                    PlateNumber = model.PlateNumber,
                    Year = model.Year,
                    OwnerId = model.OwnerId
                };

                try
                {
                    data.Cars.Add(car);
                    data.SaveChanges();
                }
                catch (SqlException e)
                {
                    errors.Add(e.Message);
                }
            }

            return errors;
        }

        public ICollection<CarViewModel> GetAllCarsForUser(string userId)
        {
            var cars = data.Cars.Where(c => c.OwnerId == userId)
                .Select(c => new CarViewModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    ImageUrl = c.PictureUrl,
                    PlateNumber = c.PlateNumber,
                    FixedIssuesCount = c.Issues.Where(i => i.IsFixed == true).Count().ToString(),
                    RemainingIssuesCount = c.Issues.Where(i => i.IsFixed == false).Count().ToString()
                })
                .ToList();

            return cars;
        }

        public ICollection<CarViewModel> GetAllCarsWithIssues()
        {
            var cars = data.Cars.Where(c => c.Issues.Any(i => i.IsFixed == false))
                .Select(c => new CarViewModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    ImageUrl = c.PictureUrl,
                    PlateNumber = c.PlateNumber,
                    FixedIssuesCount = c.Issues.Where(i => i.IsFixed == true).Count().ToString(),
                    RemainingIssuesCount = c.Issues.Where(i => i.IsFixed == false).Count().ToString()
                })
                .ToList();

            return cars;
        }

        public CarModelYearViewModel GetCarModelYear(string id)
        {
            return data.Cars.Where(c => c.Id == id).Select(c => new CarModelYearViewModel
                {
                    Model = c.Model,
                    Year = c.Year
                })
                .FirstOrDefault();
        }
    }
}
