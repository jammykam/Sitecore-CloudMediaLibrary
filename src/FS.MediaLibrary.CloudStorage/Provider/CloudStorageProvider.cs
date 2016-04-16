using FS.MediaLibrary.CloudStorage.Interfaces;
using Sitecore.Configuration;

namespace Arm.CMS.Services.Media
{
    public class CloudStorageProvider : ICloudStorageProvider
    {
        public ICloudStorage GetProvider()
        {
            return Factory.CreateObject("cloudMediaStorage/storageProvider", true) as ICloudStorage;
        }
    }
}
