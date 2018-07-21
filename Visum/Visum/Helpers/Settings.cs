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
    }
}
