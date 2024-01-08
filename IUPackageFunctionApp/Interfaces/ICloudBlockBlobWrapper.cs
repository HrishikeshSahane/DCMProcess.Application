using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUPackageFunctionApp;
using Microsoft.WindowsAzure.Storage;

namespace IUPackageFunctionApp.Interfaces
{
    public interface ICloudBlockBlobWrapper
    {
        CloudBlockBlobWrapper GetBlobReference(Uri blobAbsoluteUri, Microsoft.WindowsAzure.Storage.Auth.StorageCredentials credentials);
        Task<bool> Exists();

        Task<string> StartCopyAsync(CloudBlockBlobWrapper dest, CloudBlockBlobWrapper source);
        Task DownloadToStreamAsync(CloudBlockBlobWrapper obj, Stream target, Microsoft.WindowsAzure.Storage.AccessCondition accessCondition, Microsoft.WindowsAzure.Storage.Blob.BlobRequestOptions options, Microsoft.WindowsAzure.Storage.OperationContext operationContext);

        void DownloadToStream(CloudBlockBlobWrapper obj, Stream target, Microsoft.WindowsAzure.Storage.AccessCondition accessCondition = null, Microsoft.WindowsAzure.Storage.Blob.BlobRequestOptions options = null, Microsoft.WindowsAzure.Storage.OperationContext operationContext =null);
        string Name();
        Microsoft.WindowsAzure.Storage.Blob.CloudBlobDirectory Parent();
    }
}
