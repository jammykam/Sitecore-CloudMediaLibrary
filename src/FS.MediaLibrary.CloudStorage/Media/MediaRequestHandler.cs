using System.Web;
using FS.MediaLibrary.CloudStorage.Helpers;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;

namespace FS.MediaLibrary.CloudStorage.Media
{
    public class MediaRequestHandler : Sitecore.Resources.Media.MediaRequestHandler
    {
        protected override bool DoProcessRequest(HttpContext context)
        {
            Assert.ArgumentNotNull((object)context, "context");
            MediaRequest request = MediaManager.ParseMediaRequest(context.Request);
            if (request == null)
                return false;
            Sitecore.Resources.Media.Media media = MediaManager.GetMedia(request.MediaUri);

            if (!request.Options.Thumbnail && IsCdnMedia(media))
            {
                return this.DoProcessRequest(context, media);
            }

            return base.DoProcessRequest(context);
        }

        private bool DoProcessRequest(HttpContext context, Sitecore.Resources.Media.Media media)
        {
            var helper = new MediaHelper();
            string redirectUrl = helper.GetCloudBasedMediaUrl(media.MediaData.MediaItem);
            context.Response.Redirect(redirectUrl);
            return true;
        }

        private bool IsCdnMedia(Sitecore.Resources.Media.Media media)
        {   
            return (media != null && media.MediaData.MediaItem.FileBased);
        }
    }
}
