using Sitecore.Data.Items;

namespace FS.MediaLibrary.CloudStorage.Interfaces
{
    interface ICloudStorageProvider
    {
        //ICloudStorage GetProvider();
        
        string Put(MediaItem media);
        string Update(MediaItem media);
        bool Delete(MediaItem media);
    }
}
