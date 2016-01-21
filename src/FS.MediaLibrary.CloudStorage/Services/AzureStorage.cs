using System.Configuration;
using FS.MediaLibrary.CloudStorage.Constants;
using FS.MediaLibrary.CloudStorage.Helpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace FS.MediaLibrary.CloudStorage.Services
{
    /// <summary>
    /// Uploads media items into Azure Blob storage container
    /// </summary>
    public class AzureStorage : ICloudStorage
    {
        private CloudBlobContainer blobContainer;

        #region ctor
        public AzureStorage() :
            this(ConfigurationManager.AppSettings[AppSettings.AzureStorageAccountName],
                 ConfigurationManager.AppSettings[AppSettings.AzureStorageAccountKey],
                 ConfigurationManager.AppSettings[AppSettings.AzureStorageContainer])
        {
        }

        public AzureStorage(string accountName, string accountKey, string container)
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(container);
        }
        #endregion

        #region Implementation
        public string Put(MediaItem media)
        {
            var helper = new MediaHelper();
            string filename = helper.ParseMediaFileName(media);

            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(filename);
            using (var fileStream = media.GetMediaStream())
            {
                blob.UploadFromStream(fileStream);
            }

            Log.Info("File successfully uploaded to Azure Blob Storage: " + filename , this);

            return filename;
        }

        public string Update(MediaItem media)
        {
            return Put(media);
        }

        public bool Delete(string filename)
        {
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(filename);
            return blob.DeleteIfExists();
        }
        #endregion
    }
}
