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
            Assert.ArgumentNotNull((object) context, "context");
            MediaRequest request = MediaManager.ParseMediaRequest(context.Request);
            if (request == null)
                return false;
            Sitecore.Resources.Media.Media media = MediaManager.GetMedia(request.MediaUri);

            if (!IsCdnMedia(media))
                return base.DoProcessRequest(context);

            if (request.Options.Thumbnail)
            {
                request.Options.UseMediaCache = false;
                return base.DoProcessRequest(context, request, media);
            }

            return this.DoProcessRequest(context, media);
        }

        private bool DoProcessRequest(HttpContext context, Sitecore.Resources.Media.Media media)
        {
            var helper = new MediaHelper(media.MediaData.MediaItem);
            string redirectUrl = helper.GetCloudBasedMediaUrl();
            context.Response.Redirect(redirectUrl, false);
            context.ApplicationInstance.CompleteRequest();
            return true;
        }

        private bool IsCdnMedia(Sitecore.Resources.Media.Media media)
        {
            return (media != null && media.MediaData.MediaItem.FileBased);
        }
    }
}
