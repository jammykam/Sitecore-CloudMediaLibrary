using FS.MediaLibrary.CloudStorage.Interfaces;
using Sitecore.Configuration;

namespace FS.MediaLibrary.CloudStorage.Provider
{
    public class CloudStorageProvider : ICloudStorageProvider
    {
        public ICloudStorage GetProvider()
        {
            return Factory.CreateObject("cloudMediaStorage/storageProvider", true) as ICloudStorage;
        }
    }
}
