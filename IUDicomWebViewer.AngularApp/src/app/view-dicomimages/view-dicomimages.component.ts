import { Component, ViewChild } from '@angular/core';
import { WebImageViewerComponent } from '../../../projects/dicom-viewer/src/lib/webimage-viewer.component';
import { delay} from 'rxjs';
import { IuDicomwebviewerService } from '../services/iu-dicomwebviewer.service';
import { Router } from '@angular/router';
import axios, { AxiosHeaders } from 'axios';
import * as JSZip from 'jszip';
import { HttpHeaders } from '@angular/common/http';
declare const cornerstone;
declare const cornerstoneWADOImageLoader;
var newfiles: File[] = [];

@Component({
  selector: 'app-view-dicomimages',
  templateUrl: './view-dicomimages.component.html',
  styleUrls: ['./view-dicomimages.component.css']
})
export class ViewDicomimagesComponent {
  blobServiceUri:string='';
  @ViewChild(WebImageViewerComponent, { static: true }) viewPort: WebImageViewerComponent;
  constructor(private _IuDicomwebviewerService: IuDicomwebviewerService,private router:Router){

}

  ngOnInit() {
    this.readZip()
    console.log("Before delay")
    delay(10000);
    console.log("After delay")
    cornerstoneWADOImageLoader.external.cornerstone = cornerstone; // inicializa WADO Image loader

    // configura codecs e web workers
    cornerstoneWADOImageLoader.webWorkerManager.initialize({
        webWorkerPath: './assets/cornerstone/webworkers/cornerstoneWADOImageLoaderWebWorker.js',
        taskConfiguration: {
            'decodeTask': {
                codecsPath: '../codecs/cornerstoneWADOImageLoaderCodecs.js'
            }
        }
    });


  }

  /**
   * Load selected DICOM images
   *
   * @param files list of selected dicom files
   */
  loadDICOMImages() {  
    console.log(newfiles.length)
    if (newfiles && newfiles.length > 0) {
      let imageList = [];
      // const fileList:Array<File> = Array.from(files);
      // fileList.sort((a,b) => {
      //   if ( a.name > b.name ) return 1;
      //   if ( b.name > a.name ) return -1;
      //   return 0;
      // })
      //cornerstoneWADOImageLoader.wadouri.fileManager.purge();
      cornerstoneWADOImageLoader.wadouri.dataSetCacheManager.purge();
      newfiles=newfiles.slice(0,10);
      
        // loop thru the File list and build a list of wadouri imageIds (dicomfile:)
        for (let i = 0; i < newfiles.length; i++)
        {
          
          const dicomFile: File = newfiles[i];
          const imageId = cornerstoneWADOImageLoader.wadouri.fileManager.add(dicomFile);
          imageList.push(imageId);
        }
      
      this.viewPort.resetAllTools();
      imageList=imageList.slice(0,10);
      // now load all Images, using their wadouri
      this.viewPort.loadStudyImages(imageList);

    } else alert('Choose DICOM images to display.');
  }


  // async download(){
  //   console.log("Before SASToken call");
  //   console.log(this.blobServiceUri);
  //   const credential = undefined;
  //   const blobServiceClient = new BlobServiceClient("https://iupackagecontainer.blob.core.windows.net?sv=2023-01-03&se=2024-01-03T19%3A51%3A27Z&sr=c&sp=rwl&sig=WbLpS0WNUGV7aW4%2FX%2FgpY8GQHrUz1SnLt9FdxFXS9vY%3D", credential);

  //   console.log("Starting download")
  //   const containerName = 'dicomblob';
  //   const blobName = '2.57.299.2709.41320.615801.4965911.64446389.zip';
  
  //   const timestamp = Date.now();
  //   const fileName ="./e2e/Output.zip";
  
  //   // create container client
  //   console.log("Create containerClient")
  //   const containerClient:ContainerClient = await blobServiceClient.getContainerClient(
  //     containerName
  //   );
  
  //   // create blob client
  //   const blobClient:BlobClient = await containerClient.getBlockBlobClient(blobName);
  //   console.log("Created blob client")
  //   // download file
  //   // headers= {
  //   //   blobContentType: 'text/plain',
  //   //   blobContentLanguage: 'en-us',
  //   //   blobContentEncoding: 'utf-8',
  //   //   // all other http properties are cleared
  //   // }

  //   console.log("Downloading blob....")

  //   const downloadResponse: BlobDownloadResponseParsed =
  //   await blobClient.download();
  //   console.log("Printing response")
  //   console.log(downloadResponse)  
  //     var readableStream=downloadResponse.readableStreamBody
  //     const chunks: Buffer[] = [];

  //   readableStream.on('data', (data) => {
  //     const content: Buffer = data instanceof Buffer ? data : Buffer.from(data);
  //     chunks.push(content);
  //     const zip = new AdmZip(content);
  //     zip.getEntries().forEach((entry) => {
  //       const entryName = entry.entryName;
  //       const entryPath = path.join(entryName);
  //       console.log(entryName)

  //       if (entry.isDirectory) {
  //         fs.mkdirSync(entryPath, { recursive: true });
  //       } else {
  //         const entryContent = zip.readFile(entry);
  //         fs.writeFileSync(entryPath, entryContent, 'binary');
  //       }
  //     });
      


  //   });

  //   console.log("Blob has been Downloaded")

  // }


  async readZip(){
    var studyId:string='';
    newfiles=[];
    this._IuDicomwebviewerService.data$.subscribe((data) => {
      studyId = data['key'];
    });
    console.log("In ViewDicomimagesComponent: "+studyId);

    this._IuDicomwebviewerService.getToken().subscribe
    (
        async token=>
      {
        const headers = new AxiosHeaders({
          'Authorization': `Bearer ${token}`
        });
        //const response = await axios.get("https://localhost:7282/api/Storage/GetDicomFiles?studyId="+studyId.toString(), { responseType: 'arraybuffer',headers:headers });
        const response = await axios.get("https://dcmprocessapiservice.azurewebsites.net/api/Storage/GetDicomFiles?studyId="+studyId.toString(), { responseType: 'arraybuffer',headers:headers });


        const zip = await JSZip.loadAsync(response.data);
  
        Object.keys(zip.files).map(async (relativePath) => {
          const zipEntry = zip.file(relativePath);

          if (zipEntry) {
            const fileContent = await zipEntry.async('arraybuffer');
            const fileType = zipEntry.dir ? '' : zipEntry.name.split('.').pop() || '';

            // Create a File from the Blob
            const blob = new Blob([fileContent], { type: fileType });
            const file = new File([blob], zipEntry.name, { type: blob.type });

            console.log(file.name)
            if(newfiles.length<10){
              await newfiles.push(file);
              this.loadDICOMImages()
            }




          }
        })
      }
    );
  }
}
