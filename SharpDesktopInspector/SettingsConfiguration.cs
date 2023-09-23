using System.IO;
using System.Runtime.Serialization.Json;

namespace SharpDesktopInspector
{
    class SettingsConfiguration
    {
        public static void SaveSettings(Settings settings)
        {
            string? settingsFile = PathInfo.SettingsFile;

            DataContractJsonSerializer settingsSerializer = new(typeof(Settings));
            FileStream fileStream = new(settingsFile, FileMode.Create);
            settingsSerializer.WriteObject(fileStream, settings);
            fileStream.Close();
        }

        public static Settings? ConfigureApplicationSettings()
        {
            string? settingsFile = PathInfo.SettingsFile;

            Settings? settings = new();

            if (File.Exists(settingsFile))
            {
                DataContractJsonSerializer settingsSerializer = new(typeof(Settings));
                FileStream fileStream = new(settingsFile, FileMode.Open);

#pragma warning disable CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
                settings = (Settings)settingsSerializer.ReadObject(fileStream) as Settings;
#pragma warning restore CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
                fileStream.Close();
            }
            else
            {
                settings.Target = 0;
                settings.TargetType = string.Empty;
            }

            return settings;
        }
    }
}
