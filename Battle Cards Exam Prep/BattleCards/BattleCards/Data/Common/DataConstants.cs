namespace BattleCards.Data.Common
{
    public class DataConstants
    {
        #region User
        public const int UserMinUsernameLength = 5; 
        public const int UserMaxUsernameLength = 20;      
        public const int UserMinPasswordLength = 6; 
        public const int UserMaxPasswordLength = 20; 
        public const string UserEmailPattern = "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";
        #endregion


        #region Card
        public const int CardNameMinLength = 5;
        public const int CardNameMaxLength = 15;
        public const int CardDescriptionMaxLength = 200;
        public const int CardStatsMinValue = 0;
        #endregion
    }
}
