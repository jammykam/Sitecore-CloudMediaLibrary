using System;
using System.Security.Cryptography;
using FS.MediaLibrary.CloudStorage.Configuration;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.IO;
using Sitecore.Resources.Media;

namespace FS.MediaLibrary.CloudStorage.Helpers
{
    public class MediaHelper
    {
        private MediaItem mediaItem;

        public MediaHelper(MediaItem item)
        {
            this.mediaItem = item;
        }

        /// <summary>Calculats MD5 hash for files uploaded to media library</summary>
        /// <returns>MD5 Hash of file</returns>
        public string CalculateMd5()
        {
            byte[] hash;
            using (var md5 = MD5.Create())
            {
                using (var stream = this.mediaItem.GetMediaStream())
                {
                    hash = md5.ComputeHash(stream);
                }
            }
            return BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
        }

        /// <summary>Returns the URL of the media item stored in Cloud storage</summary>
        /// <returns>Full URL of media stoed in </returns>
        public string GetCloudBasedMediaUrl()
        {
            return GetCloudBasedMediaUrl(Settings.CloudMediaUrl);
        }

        public string GetCloudBasedMediaUrl(string cloudUrl)
        {
            return cloudUrl + StringUtil.RemovePrefix("/", this.mediaItem.FilePath);
        }

        /// <summary>
        /// Generates a thumbnail of the media item and stores to Media Thumbnail Cache folder
        /// </summary>
        public void GenerateThumbnail()
        {
            // we only care about storing generated thumnails, not default icons
            ThumbnailGenerator thumbnailGenerator = MediaManager.Config.GetThumbnailGenerator(mediaItem.Extension);
            if (!(thumbnailGenerator is ImageThumbnailGenerator))
                return;

            CreateThumbnail();
        }

        private void CreateThumbnail()
        {
            var mediaData = new MediaData(this.mediaItem);
            MediaStream thumbnailStream = mediaData.GetThumbnailStream();
            if (thumbnailStream == null)
                return;

            string file = GetThumbnailFilename();
            FileUtil.EnsureFileFolder(file);
            FileUtil.CreateFile(FileUtil.MapPath(file), thumbnailStream.Stream, true);
        }

        private string GetThumbnailFilename()
        {
            return String.Format("{0}{1}.{2}",
                                    GetThumnailCacheFolder(),
                                    mediaItem.ID.ToShortID(),
                                    mediaItem.Extension);
        }

        private string GetThumnailCacheFolder()
        {
            string folder = Settings.MediaThumbnailCacheFolder + ((byte)mediaItem.ID.GetHashCode());
            return StringUtil.EnsurePostfix('/', folder);
        }

        /// <summary>
        /// Gets Stream if Media Thumbnail file is exists in cache folder
        /// </summary>
        /// <returns>MediaStream</returns>
        public MediaStream GetThumbnailStream()
        {
            string filename = GetThumbnailFilename();
            if (FileUtil.FileExists(filename))
            {
                return new MediaStream(FileUtil.OpenRead(filename), mediaItem.Extension, mediaItem);
            }

            return null;
        }

        /// <summary>Deletes media file from disk</summary>
        public void DeleteThumbnail()
        {
            FileUtil.Delete(GetThumbnailFilename());
        }

    }
}
