using FS.MediaLibrary.CloudStorage.Provider;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace FS.MediaLibrary.AzureStorage
{
    /// <summary>
    /// Uploads media items into Azure Blob storage container
    /// </summary>
    internal class AzureStorageProvider : CloudStorageBase
    {
        private CloudBlobContainer _blobContainer;
        private string _storageAccountName;
        private string _storageAccountKey;
        private string _storageContainer;

        #region ctor
        public AzureStorageProvider(string accountName, string accountKey, string container)
        {
            _storageAccountName = accountName;
            _storageAccountKey = accountKey;
            _storageContainer = container;
            this.Initialize();
        }

        private void Initialize()
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(_storageAccountName, _storageAccountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = blobClient.GetContainerReference(_storageContainer);
        }
        #endregion


        #region Implementation
        /// <summary>
        /// Uploads the media file into Azure Storage container
        /// </summary>
        /// <param name="media">Media Item to upload</param>
        /// <returns>Location of file in container</returns>
        public override string Put(MediaItem media)
        {
            string filename = ParseMediaFileName(media);

            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(filename);
            using (var fileStream = media.GetMediaStream())
            {
                blob.UploadFromStream(fileStream);
            }

            Log.Info("File successfully uploaded to Azure Blob Storage: " + filename, this);

            return filename;
        }

        /// <summary>
        /// Overrides the existing media item with this new one
        /// </summary>
        /// <param name="media">Media Item to upload</param>
        /// <returns>Location of file in container</returns>
        public override string Update(MediaItem media)
        {
            return Put(media);
        }

        /// <summary>
        /// Delete the associated media file from Azure storage
        /// </summary>
        /// <param name="filename">Location fo file to delete in storage</param>
        /// <returns>Bool indicating success</returns>
        public override bool Delete(string filename)
        {
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(filename);
            return blob.DeleteIfExists();
        }
        #endregion
    }
}
