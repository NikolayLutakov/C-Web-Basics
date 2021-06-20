namespace CarShop.ViewModels
{
    public class CarViewModel
    {
        public string Id { get; set; }
        public string Model { get; set; }
        public string ImageUrl { get; set; }

        public string PlateNumber { get; set; }

        public string RemainingIssuesCount { get; set; }

        public string FixedIssuesCount { get; set; }
    }
}
