using Sitecore.Data.Items;

namespace FS.MediaLibrary.CloudStorage.Interface
{
    public interface ICloudStorage
    {
        string Put(MediaItem media);
        string Update(MediaItem media);
        bool Delete(string filename);
    }
}
