using Microsoft.Extensions.Configuration;

namespace CliquesForGenome.Configuration
{
    public static class SettingsLoader
    {
        private const string SETTINGS_FILE = "appsettings.json";

        public static void LoadSettings(Settings settings)
        {
            new ConfigurationBuilder()
            .AddJsonFile(SETTINGS_FILE, false, true)
            .Build().Bind(settings);
        }
    }
}
