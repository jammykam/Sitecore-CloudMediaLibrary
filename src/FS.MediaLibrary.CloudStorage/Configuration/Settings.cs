using FS.MediaLibrary.CloudStorage.Constants;
using Sitecore;

namespace FS.MediaLibrary.CloudStorage.Configuration
{
    public class Settings
    {
        public static string CloudMediaUrl
        {
            get
            {
                return Sitecore.Configuration.Settings.GetSetting(ConfigSettings.MediaLinkCdnServerUrl);
            }
        }
    }
}
