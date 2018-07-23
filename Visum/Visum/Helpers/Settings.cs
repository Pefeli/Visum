namespace Visum.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        const string token = "Token";
        const string userId = "UserId";
        const string name = "Name";
        static readonly string stringDefault = string.Empty;

        public static string Token
        {
            get
            {
                return AppSettings.GetValueOrDefault(token, stringDefault);
            }

            set
            {
                AppSettings.AddOrUpdateValue(token, value);
            }
        }

        public static string UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault(userId, stringDefault);
            }

            set
            {
                AppSettings.AddOrUpdateValue(userId, value);
            }
        }

        public static string Name
        {
            get
            {
                return AppSettings.GetValueOrDefault(name, stringDefault);
            }

            set
            {
                AppSettings.AddOrUpdateValue(name, value);
            }
        }
    }
}
