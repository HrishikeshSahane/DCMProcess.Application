using Microsoft.Identity.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMProcess.DataAccessLayer
{
    public class BlobRepository
    {
        readonly MemoryStream getZipBlobFileStream;

        public BlobRepository()
        {
            getZipBlobFileStream = new MemoryStream();
        }

        public async Task<MemoryStream> DownloadBlob(string seriesId)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                BlobRequestOptions optionWithRetryPolicy = new BlobRequestOptions();
                var storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dcmprocesscontainer;AccountKey=ns9OQoQ1LooyBXHD6OYsWKLyjZL53HUoyhz5tw5X6xK5CIYq5Qu+wqD9OiOH15ddX5K77lxiuhgh+ASt64oB5Q==;EndpointSuffix=core.windows.net";
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("dicomblob");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{seriesId}.zip");

                using (var zipBlobFileStream = new MemoryStream())
                {

                    await blockBlob.DownloadToStreamAsync(zipBlobFileStream);
                    await zipBlobFileStream.FlushAsync();
                    zipBlobFileStream.Position = 0;
                    memoryStream= zipBlobFileStream;
                    memoryStream.Seek(0, SeekOrigin.Begin);
                }

                return memoryStream;
                
            }

            catch (Exception ex)
            {
                return getZipBlobFileStream;
            }
        }
    }
}
