using Sitecore.Data.Items;

namespace FS.MediaLibrary.CloudStorage.Interface
{
    interface ICloudStorageProvider
    {
        string Put(MediaItem media);
        string Update(MediaItem media);
        bool Delete(MediaItem media);
    }
}
