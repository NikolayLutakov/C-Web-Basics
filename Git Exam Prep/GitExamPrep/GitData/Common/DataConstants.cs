using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitData.Common
{
    public class DataConstants
    {
        #region User
        public const int UserMaxUsernameLength = 20;
        public const int UserMinUsernameLength = 5;
        public static readonly string UserUsernamePattern = $"^[\\w-]{{{UserMinUsernameLength},{UserMaxUsernameLength}}}$";
        public const int UserMaxPasswordLength = 20;
        public const int UserMinPasswordLength = 6;
        public static readonly string UserPasswordPattern = $"^[\\w-]{{{UserMinPasswordLength},{UserMaxPasswordLength}}}$";
        public const string UserEmailPattern = "^[\\w-_\\.]+@([\\w-]+\\.)+[\\w-]{2,}$";

        #endregion

    }
}
