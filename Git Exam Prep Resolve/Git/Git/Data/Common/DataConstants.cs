using System;

namespace Git.Data.Common
{
    public class DataConstants
    {
        public const int UserMinUsernameLength = 5;
        public const int UserMaxUsernameLength = 20;     
        public const int UserMinPasswordLength = 6;
        public const int UserMaxPasswordLength = 20;
        public const string UserEmailPattern = "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";

        public const int RepositoryMinNameLength = 3;
        public const int RepositoryMaxNameLength = 10;

        public const int CommitMinDescriptionLength = 3;
    }

}
