using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IUPackageFunctionApp.Interfaces;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO.Compression;
using System.Net.Http;
using IUPackageFunctionApp.Models;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace IUPackageFunctionApp
{
    public class FunctionHelper
    {
        readonly Func<MemoryStream> getZipBlobFileStream;
        public string APIBaseURL = "https://localhost:7282/api/";
        HttpClient client;
        Dicom.DicomFile dicomFile;

        public FunctionHelper()
        {
            getZipBlobFileStream = () => new MemoryStream();
            client= new HttpClient();
            dicomFile=new Dicom.DicomFile();
        }

        public FunctionHelper(Func<MemoryStream> _zipBlobFileStream,Func<MemoryStream> _fileBlobFileStream, HttpClient _client)
        {
            getZipBlobFileStream = _zipBlobFileStream;  
            client = _client;
        }

        /// <summary>
        /// Process Queue Messages present in Queue Storages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task ProcessQueueMessage(PackageDetails message)
        {
            try
            {
                string fileName = String.Empty;
                UpdateDequeueMessageTime(message);
                BlobRequestOptions optionWithRetryPolicy = new BlobRequestOptions();
                Uri blobUri = new Uri(message.BlobURI);
                var storageConnectionString= "DefaultEndpointsProtocol=https;AccountName=dcmprocesscontainer;AccountKey=ns9OQoQ1LooyBXHD6OYsWKLyjZL53HUoyhz5tw5X6xK5CIYq5Qu+wqD9OiOH15ddX5K77lxiuhgh+ASt64oB5Q==;EndpointSuffix=core.windows.net"; 
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("dicomblob");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{message.SeriesID}.zip");
                
                using(var zipBlobFileStream = getZipBlobFileStream())
                {

                    await blockBlob.DownloadToStreamAsync(zipBlobFileStream);
                    await zipBlobFileStream.FlushAsync();
                    zipBlobFileStream.Position = 0;


                    using (var zip=new ZipArchive(zipBlobFileStream))
                    {
                        foreach(var fileFromZip in zip.Entries)
                        {

                            var fileStream= fileFromZip.Open();
                            fileName = fileFromZip.Name;
                            CloudBlockBlob destinationblockBlob = container.GetBlockBlobReference(fileName);
                            destinationblockBlob.Properties.ContentType = "application/dicom";
                            await destinationblockBlob.UploadFromStreamAsync(fileStream);
                            break;

                        }
                    }

                }
                ReadDicomFileFromBlob(container, fileName);
            }
            catch(Exception ex)
            {
                
            }
        }

        public async Task ReadDicomFileFromBlob(CloudBlobContainer container,string fileName)
        {
            CloudBlockBlob dicomblob = container.GetBlockBlobReference(fileName);
            using(var fileBlobFileStream=new MemoryStream())
            {
                await dicomblob.DownloadToStreamAsync(fileBlobFileStream);
                await fileBlobFileStream.FlushAsync();
                fileBlobFileStream.Seek(0, SeekOrigin.Begin);
                dicomFile = Dicom.DicomFile.Open(fileBlobFileStream);
                await ProcessPatientDetails(dicomFile);
                await ProcessImageScanDetails(dicomFile);
                await ProcessInstitutionDetails(dicomFile);
                await ProcessMachineScannerDetails(dicomFile);
                await dicomblob.DeleteIfExistsAsync();
            }

        }

        public async Task UpdateDequeueMessageTime(PackageDetails packageDetails)
        {
            try
            {
                QueueDetail queueDetail = new QueueDetail()
                {
                    BlobUri = packageDetails.BlobURI,
                    SentImageCount = Convert.ToInt32(packageDetails.FilesCount),
                    DequeueTime = DateTime.Now
                };

                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(queueDetail), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync($"{APIBaseURL}Storage/SaveQueueDetails", httpContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Http error occured with status code : {response.StatusCode} and message as {response.Content.ReadAsStringAsync().Result}");
                }
            }
            catch(Exception ex)
            {

            }

        }


        public async Task ProcessPatientDetails(Dicom.DicomFile dicomFile)
        {
            try
            {
                var dicomDataset = dicomFile.Dataset;

                var patientName = dicomDataset.GetSingleValue<string>(Dicom.DicomTag.PatientName);
                if (patientName.Contains(' '))
                {
                    patientName.Replace(' ', '^');
                }
                string birthYear= dicomDataset.GetSingleValue<string>(Dicom.DicomTag.PatientBirthDate).Substring(0,4);
                string birthMonth= dicomDataset.GetSingleValue<string>(Dicom.DicomTag.PatientBirthDate).Substring(4,2);
                string birthDay = dicomDataset.GetSingleValue<string>(Dicom.DicomTag.PatientBirthDate).Substring(6, 2);
                Patient patient = new Patient()
                {
                    
                    FirstName = patientName.Split('^').ToList().FirstOrDefault(),
                    LastName = patientName.Split('^').ToList().Skip(1).FirstOrDefault()??String.Empty,
                    PatientId = dicomDataset.GetSingleValue<string>(Dicom.DicomTag.PatientID),
                    Gender = dicomDataset.GetSingleValue<string>(Dicom.DicomTag.PatientSex)??String.Empty,
                    DateOfBirth = new DateTime(Convert.ToInt32(birthYear), Convert.ToInt32(birthMonth), Convert.ToInt32(birthDay)),
                    PatientCreatedDate=DateTime.Now
                };
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(patient), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync($"{APIBaseURL}Patient/SavePatient", httpContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Http error occured with status code : {response.StatusCode} and message as {response.Content.ReadAsStringAsync().Result}");
                }


            }
            catch(Exception ex)
            {

            }
        }

        public async Task ProcessImageScanDetails(Dicom.DicomFile dicomFile)
        {
            try
            {
                var dicomDataset = dicomFile.Dataset;
                var imageSeriesId= dicomDataset.GetSingleValue<string>(Dicom.DicomTag.StudyInstanceUID);
                var imageModality = dicomDataset.GetSingleValue<string>(Dicom.DicomTag.Modality);
                var imageType= dicomDataset.GetValues<string>(Dicom.DicomTag.ImageType);
                var patientMRN = dicomDataset.GetSingleValue<string>(Dicom.DicomTag.PatientID);
                var patientPosition = dicomDataset.GetValues<string>(Dicom.DicomTag.ImagePositionPatient);
                var patientOrientation = dicomDataset.GetValues<string>(Dicom.DicomTag.ImageOrientationPatient);
                
                var scannedDate = DateTime.Now;

                ScanDetail scanDetail = new ScanDetail()
                {
                    SeriesId = imageSeriesId,
                    ImageModality=imageModality,
                    ImageType=string.Join("/", imageType),
                    PatientMRN=patientMRN,
                    PatientPosition=string.Join("/",patientPosition),
                    PatientOrientation= string.Join("/", patientOrientation),
                    ScannedDate=scannedDate,

                    
                };
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(scanDetail), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync($"{APIBaseURL}Patient/SaveScanDetails", httpContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Http error occured with status code : {response.StatusCode} and message as {response.Content.ReadAsStringAsync().Result}");
                }

            }
            catch(Exception ex)
            {

            }
        }


        public async Task ProcessMachineScannerDetails(Dicom.DicomFile dicomFile)
        {
            try
            {
                var dicomDataset = dicomFile.Dataset;
                var imageSeriesId= dicomDataset.GetSingleValue<string>(Dicom.DicomTag.StudyInstanceUID);
                var manufacturer = dicomDataset.GetSingleValue<string>(Dicom.DicomTag.Manufacturer);
                var modelName = dicomDataset.GetSingleValue<string>(Dicom.DicomTag.ManufacturerModelName);
                var operatorName = String.Empty;
                try
                {
                    operatorName = dicomDataset.GetSingleValue<string>(Dicom.DicomTag.OperatorsName);
                }
                catch
                {
                    operatorName = String.Empty;
                }
                 
                var xRayTubeCurrent = 0;
                try
                {
                    xRayTubeCurrent = dicomDataset.GetSingleValue<int>(Dicom.DicomTag.XRayTubeCurrent);
                }
                catch
                {
                    xRayTubeCurrent = 0;
                }
                
                var createdDate= DateTime.Now;


                MachineDetail machineDetail = new MachineDetail()
                {
                    Manufacturer= manufacturer,
                    ModelName=modelName,
                    ImageSeriesId=imageSeriesId,
                    OperatorName=operatorName,
                    XrayTubeCurrent=xRayTubeCurrent,
                    CreatedDate=createdDate,
                };
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(machineDetail), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync($"{APIBaseURL}Institute/SaveMachineScannerDetails", httpContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Http error occured with status code : {response.StatusCode} and message as {response.Content.ReadAsStringAsync().Result}");
                }
            }

            
            catch(Exception ex)
            {

            }
        }


        public async Task ProcessInstitutionDetails(Dicom.DicomFile dicomFile)
        {
            try
            {
                var dicomDataset = dicomFile.Dataset;
                var institutionName = dicomDataset.GetSingleValue<string>(Dicom.DicomTag.InstitutionName);
                var departmentName = String.Empty;
                try
                {
                    departmentName = dicomDataset.GetSingleValue<string>(Dicom.DicomTag.InstitutionalDepartmentName);
                }
                catch
                {
                    departmentName = String.Empty;   
                }

                var institutionAddress= dicomDataset.GetSingleValue<string>(Dicom.DicomTag.InstitutionAddress);
                var imageSeriesId = dicomDataset.GetSingleValue<string>(Dicom.DicomTag.StudyInstanceUID);
                var createdDate = DateTime.Now;

                InstitutionDetail institutionDetail = new InstitutionDetail()
                {
                    InstitutionName = institutionName,
                    DepartmentName = departmentName,
                    InstitutionAddress = institutionAddress,
                    ImageSeriesID= imageSeriesId,
                    SeriesId=String.Empty,
                    CreatedDate= createdDate,
                };

                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(institutionDetail), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync($"{APIBaseURL}Institute/SaveInstitutionDetails", httpContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Http error occured with status code : {response.StatusCode} and message as {response.Content.ReadAsStringAsync().Result}");
                }
            }
            catch
            {

            }
        }

    }
}
