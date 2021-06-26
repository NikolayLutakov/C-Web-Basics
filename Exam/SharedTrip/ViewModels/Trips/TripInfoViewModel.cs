namespace SharedTrip.ViewModels.Trips
{
    public class TripInfoViewModel
    {
        public string Id { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public string PictureUrl { get; set; }

        public string DepartureTime { get; set; }

        public string Seats { get; set; }

        public string Description { get; set; }

        public bool IsJoined { get; set; }

        public bool HasSeats { get; set; }
    }
}
