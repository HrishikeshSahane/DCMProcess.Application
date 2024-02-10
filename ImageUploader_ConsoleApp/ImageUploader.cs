using Dicom;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Azure.Storage;
using Microsoft.Azure.Storage.DataMovement;
using System.Net;
using Microsoft.WindowsAzure.Storage.Queue;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Newtonsoft.Json;
using System.Net.Http;
using Azure.Storage.Blobs;

namespace DCMProcess.ImageUploaderApp
{
    public  class ImageUploader
    {
        public static string basePath = String.Empty;
        public static string uploadPath = String.Empty;
        public static string filePath = String.Empty;
        public static string[] seriesIdArr = new string[] { };
        public static string containerName= ConfigurationManager.AppSettings["ContainerName"];
        public static string accountName= ConfigurationManager.AppSettings["StorageAccountName"];
        public static int filesCount= 0;
        public static string storageConnectionString;
        public static string storageConnectionKey;

        public static void ReadFiles(string keyvalue1,string keyvalue2)
        {
            try
            {
                storageConnectionString = keyvalue1;
                storageConnectionKey = keyvalue2;
                Console.WriteLine($"storageConnectionString: {storageConnectionString}");
                Console.WriteLine($"storageConnectionKey: {storageConnectionKey}");
                basePath = ConfigurationManager.AppSettings["DICOMFilesPath"]; ;
                seriesIdArr = Directory.GetDirectories(basePath).Select(Path.GetFileName).ToArray();
                foreach(string s in seriesIdArr)
                {
                    filePath = Path.Combine(basePath, s);
                    Console.WriteLine($"Image Series Id: {s}");
                    string[] files = Directory.GetFiles(filePath, "*.dcm");
                    Console.WriteLine($"Total number of files to be processed: {files.Count()}");
                    filesCount = files.Count();
                    PrepareAndTransferZip(files,s);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Some error occured");
            }
            
        }

        public static void PrepareAndTransferZip(string[] files,string seriesId)
        {
            try
            {
                Console.WriteLine("Creating zip file . . .");
                DateTime startTime = DateTime.Now;
                Stream zipBlobPath = File.Create(Path.Combine(basePath, seriesId) + ".zip");
                using (var stream = new ZipArchive(zipBlobPath, ZipArchiveMode.Create))
                {
                    foreach (string s in files)
                    {

                        var dicomFile = DicomFile.Open(s);

                        if (!Path.GetExtension(s).Equals(".zip"))
                        {
                            var entry = stream.CreateEntry(seriesId + "/" + Path.GetFileName(s));
                            if (IsEntrySafe(entry))
                            {
                                using (var es = entry.Open())
                                {
                                    using (var fs = File.OpenRead(s))
                                    {
                                        fs.CopyTo(es);
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine($"Zip Successfully created in {DateTime.Now.Subtract(startTime).TotalMilliseconds} milliseconds");

                DateTime transferStartTime = DateTime.Now;
                //Get Blob Token
                Console.WriteLine($"Fetching Blob Token for transferring Zip to Blob");
                var blobURI = GetBlobSASToken();
                Console.WriteLine($"Blob Token Generated Successfully");

                //Transfer Zip to Blob
                Console.WriteLine($"Started Transferring Zip to Blob");
                TransferZip(blobURI,seriesId);
                Console.WriteLine($"Started Transferred to Blob successfully");

                Console.WriteLine($"Zip Successfully transferred in {DateTime.Now.Subtract(transferStartTime).TotalMilliseconds} milliseconds");


                //Create Queue Token
                Console.WriteLine($"Fetching Queue Token for creating message in Queue");
                var queueSasURI = GetQueueSASToken();
                Console.WriteLine($"Queue Token Generated Successfully");

                //Create Queue Message
                Console.WriteLine($"Started Creating Queue message");
                CreateQueueMessage(blobURI, queueSasURI,seriesId);
                Console.WriteLine($"Queue message created successfully");


                //Delete zip after transfer
                string zipFilePath = Path.Combine(basePath, seriesId) + ".zip";
                if (File.Exists(zipFilePath))
                {
                    File.Delete(zipFilePath);
                    Console.WriteLine($"Zip file deleted: {zipFilePath}");
                }
                else
                {
                    Console.WriteLine($"Zip file does not exist: {zipFilePath}");
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Some error occured");

            }


        }

        private static bool IsEntrySafe(ZipArchiveEntry entry)
        {
            if (entry != null)
            {
                var fullname = entry.FullName;
                var name = entry.Name;
                var isNotDirectory = !fullname.EndsWith(@"/") && !fullname.EndsWith(@"\") && !string.IsNullOrEmpty(name);
                var isNotReferringParentDirectory = !fullname.Contains("..");
                return isNotDirectory && isNotReferringParentDirectory;    
            }
            return false;
        }

        public static string GetBlobSASToken()
        {
            string BlobContainerName = containerName;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            
            CloudBlobClient blobClient=storageAccount.CreateCloudBlobClient();
            
            CloudBlobContainer container=blobClient.GetContainerReference(BlobContainerName);
            SharedAccessBlobPolicy sasContraints = new SharedAccessBlobPolicy();
            sasContraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List;
            //string sasContainerToken=container.GetSharedAccessSignature(sasContraints,null,SharedAccessProtocol.HttpsOnly,null);

            Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder()
            {
                BlobContainerName = containerName,
                //Let SAS token expire after 5 minutes.
                ExpiresOn = DateTime.UtcNow.AddMinutes(5),
            };
            blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read | Azure.Storage.Sas.BlobSasPermissions.Write| Azure.Storage.Sas.BlobSasPermissions.List);//User will only be able to read the blob and it's properties
            
            string sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(accountName, storageConnectionKey)).ToString()??String.Empty;
            string sasUrl = (container.Uri+ "?" + sasToken)??String.Empty;
            Console.WriteLine($"Token: {sasUrl}");
            return sasUrl;
        
        }

        public static string GetQueueSASToken()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            
            var queue = queueClient.GetQueueReference("package");
            var sasPolicy = new SharedAccessQueuePolicy
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),
                Permissions = SharedAccessQueuePermissions.Read |
                              SharedAccessQueuePermissions.Add | SharedAccessQueuePermissions.Update |
                              SharedAccessQueuePermissions.ProcessMessages,
                SharedAccessStartTime = DateTime.UtcNow.Subtract(new TimeSpan(0, 5, 0))
            };

            string queueSASToken= queue.GetSharedAccessSignature(sasPolicy);
            string queueSASUrl=(queue.Uri+ queueSASToken)??String.Empty;
            return queueSASUrl;
        }

        public static void ReadDicomData()
        {
            var dicomFile = DicomFile.Open(@"D:\Hrishikesh\IUBH\Elective2\Project\Files\7.41.363.3661.65759.704510.5929532.47692401\CT.55B3893E-7F47-480D-AF00-AA9302B34A20.dcm");

            var dicomDataset = dicomFile.Dataset;
            var patientName = dicomDataset.GetSingleValue<string>(DicomTag.PatientName);

        }

        public async static Task TransferZip(string blobUri,string seriesId)
        {
            try
            {
                Microsoft.Azure.Storage.Blob.CloudBlobContainer container = new Microsoft.Azure.Storage.Blob.CloudBlobContainer(new Uri(blobUri));
                string fileName = string.Concat(seriesId, ".zip");
                string localPathWithFileName = Path.Combine(basePath, fileName);
                string blobName = fileName.Replace("%", "-");
                Microsoft.Azure.Storage.Blob.CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
                blob.Properties.ContentType = "application/zip";
                int parallelTasks = Environment.ProcessorCount * 8;
                TransferManager.Configurations.ParallelOperations = parallelTasks;
                ServicePointManager.DefaultConnectionLimit = parallelTasks;
                UploadOptions options = new UploadOptions();
                SingleTransferContext context = new SingleTransferContext();
                context.ShouldOverwriteCallbackAsync = TransferContext.ForceOverwrite;
                TransferManager.UploadAsync(localPathWithFileName, blob, options, context).Wait();




                //BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);
                //BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                //BlobClient blobClient = containerClient.GetBlobClient(blobName);
                //using (FileStream fs = File.OpenRead(localPathWithFileName))
                //{
                //    await blobClient.UploadAsync(fs, true);
                //    fs.Close();
                //}

            }
            catch(Exception ex) 
            {
                Console.WriteLine("Some error occured");
            }
        }

        public static void CreateQueueMessage(string blobUri,string queueSASUrl,string seriesId)
        {
            try
            {
                PackageDetails packageDetails = new PackageDetails();
                CloudQueue queue = new CloudQueue(new Uri(queueSASUrl));
                packageDetails.SeriesID = seriesId;
                packageDetails.BlobURI = blobUri;
                packageDetails.FilesCount = filesCount;
                packageDetails.CurrentTime = DateTime.Now;
                CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(packageDetails));
                queue.AddMessageAsync(message).Wait();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Some error occured");
            }

        }

        public static string GetKeyVaultSecretValue(string key)
        {
            string value = String.Empty;
            KeyVaultDetails keyVaultDetails = new KeyVaultDetails();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = ConfigurationManager.AppSettings["APIURL"];

                    HttpResponseMessage response = client.GetAsync($"{apiUrl}KeyVault/GetSecret?key={key}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        value=response.Content.ReadAsStringAsync().Result;
                        keyVaultDetails= JsonConvert.DeserializeObject<KeyVaultDetails>(value);
                        Console.WriteLine(value);
                    }
                    return keyVaultDetails.Value;
                }
            }
            catch
            {
                return keyVaultDetails.Value;
            }
        }

        //public static string GetAzureSecret(string secretName)
        //{
        //    string keyVaultName = ConfigurationManager.AppSettings["KeyVaultName"];
        //    var kvUri = "https://" + keyVaultName + ".vault.azure.net";
        //    var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
        //    var secret = client.GetSecret(secretName);
        //    return secret.Value.ToString();
        //}
    }
}
