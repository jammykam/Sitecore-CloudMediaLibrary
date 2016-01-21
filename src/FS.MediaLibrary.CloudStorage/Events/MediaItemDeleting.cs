using System;
using FS.MediaLibrary.CloudStorage.Constants;
using FS.MediaLibrary.CloudStorage.Services;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;

namespace FS.MediaLibrary.CloudStorage.Events
{
    public class MediaItemDeleting
    {
        public void OnItemDeleting(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            Item item = Event.ExtractParameter(args, 0) as Item;
            DeleteAzureMediaBlob(item);
        }

        private void DeleteAzureMediaBlob(Item item)
        {
            if (!item.Paths.IsMediaItem)
                return;

            var media = new MediaItem(item);

            if (media.FileBased)
            {
                ICloudStorage storage = new AzureStorage();
                string filename = item[FieldNameConstants.MediaItem.FilePath];
                storage.Delete(filename);
            }
        }
    }
}
