using FS.MediaLibrary.CloudStorage.Helpers;
using Sitecore.Resources.Media;

namespace FS.MediaLibrary.CloudStorage.Pipelines.GetMediaStream
{
    /// <summary>
    /// Gets the thumbnail of cloud based media stored on disk 
    /// which was previously created before upload
    /// </summary>
    public class FileBasedThumbnailProcessor
    {
        public void Process(GetMediaStreamPipelineArgs args)
        {
            if (!ShouldProcess(args))
                return;

            var helper = new MediaHelper(args.MediaData.MediaItem);
            MediaStream thumbnailStream = helper.GetThumbnailStream();

            if (thumbnailStream != null)
            {
                args.OutputStream = thumbnailStream;
                args.AbortPipeline();
            }
        }

        private bool ShouldProcess(GetMediaStreamPipelineArgs args)
        {
            return (args.Options.Thumbnail 
                        && args.MediaData.MediaItem.FileBased 
                        && !string.IsNullOrEmpty(args.MediaData.MediaItem.Icon) 
                        && args.MediaData.MediaItem.Icon.StartsWith(Sitecore.Configuration.Settings.Media.MediaLinkPrefix));
        }
    }
}
