namespace CarShop.Data.Common
{
    public class DataConstants
    {
        public const int MaxLengthTwenty = 20;

        public const int UserMinUsernameLength = 4;
        public const int UserMinPasswordLength = 5;
        public const string UserTypeClient = "Client";
        public const string UserTypeMechanic = "Mechanic";

        public const int CarModelMinLength = 5;
        public const string CarPlateNumberPattern = "^[A-Z]{2} ?[0-9]{4} ?[A-Z]{2}$";

        public const int IssueDescriptionMinLength = 5;
    }
}
