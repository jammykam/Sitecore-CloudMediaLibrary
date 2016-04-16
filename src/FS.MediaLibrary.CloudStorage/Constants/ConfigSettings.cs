using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FS.MediaLibrary.CloudStorage.Constants
{
    public class ConfigSettings
    {
        // Custom Config Setting Keys
        public const string MediaLinkCdnServerUrl = "CloudMedia.MediaLinkCdnServerUrl";
        public const string MediaThumbnailCacheFolder = "Media.ThumbnailCacheFolder";
        public const string AlwaysIncludeCdnServerUrl = "Media.AlwaysIncludeCdnServerUrl";
    }
}