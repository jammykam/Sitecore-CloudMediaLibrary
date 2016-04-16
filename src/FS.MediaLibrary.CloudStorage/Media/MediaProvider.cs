using FS.MediaLibrary.CloudStorage.Configuration;
using FS.MediaLibrary.CloudStorage.Helpers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;

namespace FS.MediaLibrary.CloudStorage.Media
{
    public class MediaProvider: Sitecore.Resources.Media.MediaProvider
    {
        public override string GetMediaUrl(MediaItem item)
        {
            Assert.ArgumentNotNull((object)item, "item");
            return this.GetMediaUrl(item, MediaUrlOptions.Empty);
        }

        public override string GetMediaUrl(MediaItem item, MediaUrlOptions options)
        {
            Assert.ArgumentNotNull((object)item, "item");
            Assert.ArgumentNotNull((object)options, "options");

            if (!Settings.AlwaysIncludeCdnServerUrl || !item.FileBased || options.Thumbnail)
                return base.GetMediaUrl(item, options);

            var helper = new MediaHelper(item);
            return helper.GetCloudBasedMediaUrl();
        }

        public override bool HasMediaContent(Item item)
        {
            var mi = new MediaItem(item);
            if (mi.FileBased && item[Constants.FieldNameConstants.MediaItem.UploadedToCloud] == "1")
                return true;

            return base.HasMediaContent(item);
        }
    }
}
