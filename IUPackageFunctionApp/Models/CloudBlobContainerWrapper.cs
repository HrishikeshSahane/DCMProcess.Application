using IUPackageFunctionApp.Interfaces;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUPackageFunctionApp;

namespace IUPackageFunctionApp.Models
{
    public class CloudBlobContainerWrapper:ICloudBlobContainerWrapper
    {
        Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer _blobContainer;
        Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob _cloudBlockBlob;

        public CloudBlobContainerWrapper()
        {

        }

        public CloudBlobContainerWrapper(Uri uri)
        {
            _blobContainer=new Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer(uri);
        }

        public Uri GetUri(string path)
        {
            return new Uri(path);
        }

        public CloudBlobContainerWrapper GetCloudBlobContainer(Uri uri)
        {
            return new CloudBlobContainerWrapper(uri);
        }

        public CloudBlockBlobWrapper GetBlockBlobReference(string blobName)
        {
            _cloudBlockBlob = _blobContainer.GetBlockBlobReference(blobName);
            return new CloudBlockBlobWrapper(_cloudBlockBlob);
        }
    }
}
