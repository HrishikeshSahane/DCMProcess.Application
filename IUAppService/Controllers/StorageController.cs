using Microsoft.AspNetCore.Mvc;
using Azure.Storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using DCMProcess.DataAccessLayer;
using Microsoft.AspNetCore.Authorization;

namespace DCMProcess.AppService
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : Controller
    {
        private readonly DbprojectContext _context;
        DataRepository repObj;
        BlobRepository blobObj;

        public StorageController(DbprojectContext context)
        {
            _context = context;
            repObj = new DataRepository(_context);
            blobObj = new BlobRepository();
        }


        [HttpPut]
        [Route("SaveQueueDetails")]
        public async Task<JsonResult> SaveQueueDetails([FromBody]QueueDetail queueDetail)
        {
            try
            {
                if (queueDetail != null)
                {
                    bool result = repObj.SaveQueueDetails(queueDetail);
                    if (result)
                    {
                        return Json("Success!\nQueue Details Saved Successfully");
                    }
                    else
                    {
                        return Json("Failed to Save Queue Details");
                    }
                }
                return Json("No Queue Details to Save");

            }
            catch (Exception ex)
            {
                return Json($"Some error occured.\nException Details : {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetBlobSasToken")]
        public async Task<JsonResult> GetBlobSasToken()
        {
            try
            {
                string containerName = "dicomblob";
                string BlobContainerName = containerName;
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=iupackagecontainer;AccountKey=Z4UcFW6DFpWxa0dMX27N5CC+ZlgKCeIrdMcrFgYT5aO9k18f8XxhlaxZIXpA04PUcuR7iI8Av0YC+AStIsnxzw==;EndpointSuffix=core.windows.net");

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer container = blobClient.GetContainerReference(BlobContainerName);
                SharedAccessBlobPolicy sasContraints = new SharedAccessBlobPolicy();
                sasContraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List;
                //string sasContainerToken=container.GetSharedAccessSignature(sasContraints,null,SharedAccessProtocol.HttpsOnly,null);

                Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    //Let SAS token expire after 5 minutes.
                    ExpiresOn = DateTime.UtcNow.AddMinutes(10),
                };
                blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read | Azure.Storage.Sas.BlobSasPermissions.Write | Azure.Storage.Sas.BlobSasPermissions.List);//User will only be able to read the blob and it's properties

                string sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential("iupackagecontainer", "Z4UcFW6DFpWxa0dMX27N5CC+ZlgKCeIrdMcrFgYT5aO9k18f8XxhlaxZIXpA04PUcuR7iI8Av0YC+AStIsnxzw==")).ToString() ?? String.Empty;
                string sasUrl = (container.Uri + "?" + sasToken) ?? String.Empty;
                Console.WriteLine($"Token: {sasUrl}");
                return Json(sasToken);
            }
            catch (Exception ex)
            {
                return Json($"Some error occured.\nException Details : {ex.Message}");
            }
        }


        [HttpGet]
        [Authorize]
        [Route("GetDicomFiles")]
        public async Task<FileContentResult> GetDicomFiles(string studyId)
            {
            try
            {
                string zipPath = @"D:\Hrishikesh\IUBH\Thesis\PoC\Patient\5.42.357.8724.22501.922652.5648347.72768771.zip";
                MemoryStream memoryStream = new MemoryStream();
                using (FileStream fileStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(memoryStream);

                }
                //memoryStream =await blobObj.DownloadBlob(studyId);
                memoryStream.Seek(0,SeekOrigin.Begin);
                return File(memoryStream.ToArray(), "application/zip", "dicom_files.zip");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
