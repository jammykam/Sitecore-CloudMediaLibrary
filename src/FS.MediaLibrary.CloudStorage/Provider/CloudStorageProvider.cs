using FS.MediaLibrary.CloudStorage.Helpers;
using FS.MediaLibrary.CloudStorage.Interface;
using Sitecore.Configuration;
using Sitecore.Data.Items;

namespace FS.MediaLibrary.CloudStorage.Provider
{
    public class CloudStorageProvider : ICloudStorageProvider
    {
        private ICloudStorage Provider;

        public CloudStorageProvider()
        {
            Provider = Factory.CreateObject("cloudMediaStorage/storageProvider", true) as ICloudStorage;
        }

        public string Put(MediaItem media)
        {
            return Provider.Put(media);
        }

        public string Update(MediaItem media)
        {
            return Provider.Update(media);
        }

        public bool Delete(MediaItem media)
        {

            var mediaHelper = new MediaHelper(media);
            mediaHelper.DeleteThumbnail();

            return Provider.Delete(media.FilePath);
        }
    }
}
