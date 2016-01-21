using System.Diagnostics;
using System.Linq;
using FS.MediaLibrary.CloudStorage.Constants;
using FS.MediaLibrary.CloudStorage.Helpers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;

namespace FS.MediaLibrary.CloudStorage.Pipelines.MediaProcessor
{
    /// <summary>
    /// Calculates MD5 hash of uploaded file and stores in media template
    /// </summary>
    public class CalculateFileHash // : IMediaProcessor
    {
        public void Process(MediaProcessorArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Log.Info("Processing file MD5 calculation", this);
            var helper = new MediaHelper();
            var sw = new Stopwatch();
            sw.Start();

            foreach (Item file in args.UploadedItems.Where(file => file.Paths.IsMediaItem))
            {
                Profiler.StartOperation("Calculating MD5 hash for " + file.Paths.FullPath);

                using (new EditContext(file, SecurityCheck.Disable))
                {
                    file[FieldNameConstants.MediaItem.MD5Hash] = helper.CalculateMd5((MediaItem)file);
                }

                Profiler.EndOperation();
            }

            sw.Stop();
            Log.Info("Finished calculating MD5 hash for files: " + sw.Elapsed, this);
        }
    }
}
