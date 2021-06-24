using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Data.Common
{
    public class DataConstants
    {
        #region User
        public const int UserMinUsernameLength = 4;
        public const int UserMaxUsernameLength = 20;  
        public const int UserMinPasswordLength = 5;
        public const int UserMaxPasswordLength = 20;
        public const string UserEmailPattern = "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";
        #endregion


        #region Car
        public const int CarModelMinLength = 5;
        public const int CarModelMaxLength = 20;
        public const string PlateNumberPattern = "^[A-Z]{2} ?[0-9]{4} ?[A-Z]{2}$";
        #endregion


        #region Issue
        public const int IssueDescriptionMinLength = 5;
        #endregion

    }

}
