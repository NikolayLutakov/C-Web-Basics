namespace SharedTrip.Data.Common
{ 
    public class DataConstants
    { 
        public const int UserMinUsernameLength = 5;
        public const int UserMaxUsernameLength = 20;       
        public const int UserMinPasswordLength = 6;
        public const int UserMaxPasswordLength = 20;
        public const string UserEmailPattern = "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";

        public const int TripSeatsMinValue = 2;
        public const int TripSeatsMaxValue = 6;
        public const int TripDescriptionMaxLength = 80;
        public const string TripDepartureTimeFormat = "dd.MM.yyyy HH:mm";
    }

}
