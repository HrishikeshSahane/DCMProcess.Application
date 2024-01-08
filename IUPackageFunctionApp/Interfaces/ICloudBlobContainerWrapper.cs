using IUPackageFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUPackageFunctionApp;

namespace IUPackageFunctionApp.Interfaces
{
    public interface ICloudBlobContainerWrapper
    {
        CloudBlobContainerWrapper GetCloudBlobContainer(Uri uri);
        CloudBlockBlobWrapper GetBlockBlobReference(string blobName);
        Uri GetUri(string path);
    }
}
