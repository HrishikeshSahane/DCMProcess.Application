using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using IUPackageFunctionApp.Interfaces;

namespace IUPackageFunctionApp
{
    public class CloudBlockBlobWrapper : ICloudBlockBlobWrapper
    {
        private CloudBlockBlob cloudBlockBlob;
        public CloudBlockBlob _cloudBlockBlob
        {
            get { return cloudBlockBlob; }
            set { cloudBlockBlob = value; }
        }
        private CloudFile _cloudFile;

        public CloudFile cloudFile
        {
            get { return _cloudFile; }
            set { _cloudFile = value; }
        }

        private Uri _blobUri;

        public Uri blobUri
        {
            get { return _blobUri; }
            set { _blobUri = value; }
        }

        private StorageCredentials credentials;

        public StorageCredentials _credentials
        {
            get { return credentials; }
            set { credentials = value; }
        }

        public CloudBlockBlobWrapper() { }
        public CloudBlockBlobWrapper(Uri blobAbsoluteUri, StorageCredentials credentials)
        {

            blobUri = blobAbsoluteUri;
            _credentials = credentials;

            _cloudBlockBlob = new CloudBlockBlob(blobUri, _credentials);
        }

        public CloudBlockBlobWrapper(CloudBlockBlob cloudBlockBlob)
        {
            _cloudBlockBlob = cloudBlockBlob;
        }

        public CloudBlockBlobWrapper GetBlobReference(Uri blobAbsoluteUri, StorageCredentials credentials)
        {
            return new CloudBlockBlobWrapper(blobAbsoluteUri, credentials);
        }
        public Task<bool> Exists()
        {
            return _cloudBlockBlob.ExistsAsync();
        }

        public string Name()
        {
            return _cloudBlockBlob.Name;
        }
        public CloudBlobDirectory Parent()
        {
            return _cloudBlockBlob.Parent;
        }
        public async Task<string> StartCopyAsync(CloudBlockBlobWrapper dest, CloudBlockBlobWrapper source)
        {
            _cloudBlockBlob = source._cloudBlockBlob;

            return await dest._cloudBlockBlob.StartCopyAsync(_cloudBlockBlob);
        }

        internal CloudBlockBlob GetBlobReference()
        {
            return _cloudBlockBlob;
        }

        public Task DownloadToStreamAsync(CloudBlockBlobWrapper obj, Stream target, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return obj._cloudBlockBlob.DownloadToStreamAsync(target, accessCondition, options, operationContext);
        }

        //public Task<bool> DeleteIfExistsAsync(CloudBlockBlobWrapper obj, DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        //{
        //    return obj._cloudBlockBlob.DeleteIfExistsAsync(deleteSnapshotsOption, accessCondition, options, operationContext);
        //}

        //public Task UploadFromStreamAsync(CloudBlockBlobWrapper obj, Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        //{
        //    return obj._cloudBlockBlob.UploadFromStreamAsync(source, accessCondition, options, operationContext);
        //}

        public void DownloadToStream(CloudBlockBlobWrapper obj, Stream target, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
        {
            obj._cloudBlockBlob.DownloadToStreamAsync(target, accessCondition, options, operationContext);
        }
    }
}