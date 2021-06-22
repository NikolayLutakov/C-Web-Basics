namespace BattleCards.Data.Common
{
    public class DataConstants
    {
        #region User

        public const int UserMinUsernameLength = 5; 
        public const int UserMaxUsernameLength = 20;      
        public static readonly string UserUsernamePattern = $"^[\\w-]{{{UserMinUsernameLength},{UserMaxUsernameLength}}}$";
        public const int UserMinPasswordLength = 6; 
        public const int UserMaxPasswordLength = 20; 
        public static readonly string UserPasswordPattern = $"^[\\w-]{{{UserMinPasswordLength},{UserMaxPasswordLength}}}$";
        public const string UserEmailPattern = "^[\\w-_\\.]+@([\\w-]+\\.)+[\\w-]{2,}$";

        #endregion


        #region Card

        public const int CardNameMinLength = 5;
        public const int CardNameMaxLength = 15;
        public const int CardDescriptionMaxLength = 200;

        #endregion
    }
}
