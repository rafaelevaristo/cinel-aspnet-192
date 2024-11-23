namespace mvc
{
    public static class MVCConstants
    {
         public readonly struct USERS
        {
            public readonly struct ADMIN
            {
                public static readonly string USERNAME = "admin@sms.pt";
                public static readonly string EMAIL = "admin@sms.pt";
                public static readonly string PASSWORD = "xpto1234";
            }

            public readonly struct DRIVER
            {
                public static readonly string USERNAME = "driver@sms.pt";
                public static readonly string EMAIL = "driver@sms.pt";
                public static readonly string PASSWORD = "01012024";
            }

            public readonly struct ADMINISTRATIVE
            {
                public static readonly string USERNAME = "administrative@sms.pt";
                public static readonly string EMAIL = "administrative@sms.pt";
                public static readonly string PASSWORD = "qwerty1234";
            }
        }

        public readonly struct ROLES
        {
            public const string ADMIN = "ADMIN";
            public const string DRIVER = "DRIVER";
            public const string ADMINISTRATIVE = "ADMINISTRATIVE";
        }
    }
}