using System.IO;

namespace SharpDesktopInspector
{
    internal class PathInfo
    {
        public static string AppPath => typeof(App).Assembly.Location;

        public static string? AppLocation => Path.GetDirectoryName(AppPath);

        public static string? SettingsFile => $"{AppLocation}\\settings.json";
    }
}
