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
            Assert.ArgumentNotNull((object) item, "item");
            Assert.ArgumentNotNull((object) options, "options");

            if (!item.FileBased || options.Thumbnail)
                return base.GetMediaUrl(item, options);

            var helper = new MediaHelper();
            return helper.GetCloudBasedMediaUrl(item);
        }
    }
}
