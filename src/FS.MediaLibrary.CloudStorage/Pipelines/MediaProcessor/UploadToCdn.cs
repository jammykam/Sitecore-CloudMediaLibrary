using System.Diagnostics;
using System.Linq;
using FS.MediaLibrary.CloudStorage.Constants;
using FS.MediaLibrary.CloudStorage.Helpers;
using FS.MediaLibrary.CloudStorage.Services;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;

namespace FS.MediaLibrary.CloudStorage.Pipelines.MediaProcessor
{
    /// <summary>
    /// Uploads media item to azure cloud storage
    /// </summary>
    public class UploadToCdn // : IMediaProcessor
    {
        ICloudStorage storage = new AzureStorage();
        MediaHelper helper = new MediaHelper();

        public void Process(MediaProcessorArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Log.Info("Processing file upload to CDN", this);
            var sw = new Stopwatch();
            sw.Start();

            foreach (Item file in args.UploadedItems.Where(file => file.Paths.IsMediaItem))
            {
                /* NOTE: We don't deal with versioned files, should prepend file.Language and file.Version... */

                // delete if previously uploaded
                if (MainUtil.GetBool(file[FieldNameConstants.MediaItem.UploadedToCloud], false))
                    storage.Delete(file[FieldNameConstants.MediaItem.FilePath]);

                // upload to CDN
                string filename = storage.Put(file);
                
                // delete the existing file from disk
                helper.DeleteFile(file[FieldNameConstants.MediaItem.FilePath]);

                // update the item file location to CDN
                using (new EditContext(file, SecurityCheck.Disable))
                {
                    file[FieldNameConstants.MediaItem.FilePath] = filename;
                    file[FieldNameConstants.MediaItem.UploadedToCloud] = "1";
                }
            }

            sw.Stop();
            Log.Info("File Upload process to CDN complete: " + sw.Elapsed, this);
        }
    }
}
