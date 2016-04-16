using FS.MediaLibrary.CloudStorage.Interfaces;
using FS.MediaLibrary.CloudStorage.Provider;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Attach;
using Sitecore.StringExtensions;

namespace FS.MediaLibrary.CloudStorage.Pipelines.attachFile
{
    public class DeletePreviousMedia
    {
        /// <summary>
        /// Deletes media from Cloud storage that was previously associated with item
        /// </summary>
        /// <param name="args"></param>
        public void Process(AttachArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (!args.MediaItem.FileBased)
                return;

            Log.Info("Deleting '{0}' from Cloud storage".FormatWith(args.MediaItem.FilePath), this);
            
            ICloudStorage storage = new CloudStorageProvider().GetProvider();
            storage.Delete(args.MediaItem.FilePath);
        }
    }
}
