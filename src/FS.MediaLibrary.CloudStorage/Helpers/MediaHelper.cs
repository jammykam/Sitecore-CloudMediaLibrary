using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using FS.MediaLibrary.CloudStorage.Constants;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using Sitecore.StringExtensions;

namespace FS.MediaLibrary.CloudStorage.Helpers
{
    public class MediaHelper
    {
        /// <summary>
        /// Calculats MD5 hash for files uploaded to media library
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        public string CalculateMd5(MediaItem media)
        {
            byte[] hash;
            using (var md5 = MD5.Create())
            {
                using (var stream = media.GetMediaStream())
                {
                    hash = md5.ComputeHash(stream);
                }
            }
            return BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
        }


        /// <summary>
        /// Deletes media file from disk
        /// </summary>
        /// <param name="media"></param>
        public void DeleteFile(string media)
        {
            string filepath = HostingEnvironment.MapPath(media);

            if (File.Exists(filepath))
            {
                try
                {
                    File.Delete(filepath);
                }
                catch (IOException ioe)
                {
                    Log.Warn(ioe.Message, ioe, this);
                }
            }
        }


        /// <summary>
        /// Extracts the filepath after the prefix link of the media item from the media url
        /// </summary>
        /// <param name="media">MediaItem</param>
        /// <returns>string filepath</returns>
        public string ParseMediaFileName(MediaItem media)
        {
            string filename = MediaManager.GetMediaUrl(media);
            string mediaPrefix = Sitecore.StringUtil.EnsurePostfix('/', Settings.Media.MediaLinkPrefix);

            Regex regex = new Regex(@"(?<={0}).+".FormatWith(mediaPrefix));
            Match match = regex.Match(filename);
            if (match.Success)
                filename = match.Value;

            return filename;
        }


        /// <summary>
        /// Returns the URL of the media item stored in Cloud storage
        /// </summary>
        /// <param name="mediaItem">The media item</param>
        /// <returns>Full URL of media stoed in </returns>
        public string GetCloudBasedMediaUrl(MediaItem mediaItem)
        {
            return GetCloudBasedMediaUrl(mediaItem,
                    Sitecore.StringUtil.EnsurePostfix('/', ConfigurationManager.AppSettings[AppSettings.AzureStorageUrl]));
        }

        public string GetCloudBasedMediaUrl(MediaItem mediaItem, string cloudUrl)
        {
            return cloudUrl + mediaItem.FilePath;
        }
    }
}
