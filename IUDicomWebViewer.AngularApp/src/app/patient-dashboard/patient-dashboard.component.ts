import { AfterViewInit, Component, OnInit, ViewChild, isStandalone } from '@angular/core';
import { IuDicomwebviewerService } from '../services/iu-dicomwebviewer.service';
import { Router } from '@angular/router';
import { Patient} from '../models/Patient';
import { MessageService } from 'primeng/api';
import { IPatient } from '../interfaces/IPatient';
import { faImage } from '@fortawesome/free-solid-svg-icons';
import { faInfoCircle } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ScanDetail } from '../models/ScanDetail';
import { InstitutionDetail } from '../models/InstitutionDetail';


@Component({
  selector: 'app-patient-dashboard',
  templateUrl: './patient-dashboard.component.html',
  styleUrls: ['./patient-dashboard.component.css'],
  providers:[MessageService],  

})
export class PatientDashboardComponent implements OnInit{
  scanDetail=new ScanDetail();
  institutionDetail=new InstitutionDetail();
  patientId:string='';
  selectedStudyId:string=''
  fullName:string='';
  sidebarVisible: boolean = false;
  patientList!:Patient[];
  faImage=faImage;
  faInfoCircle=faInfoCircle;
  constructor(private _IuDicomwebviewerService: IuDicomwebviewerService,private messageService: MessageService,private router: Router)
  {

  }

  ngOnInit(): void {
    this.patientList=new Array();
    this._IuDicomwebviewerService.getToken().subscribe(
      token=>{
        this._IuDicomwebviewerService.getAllPatients(token).subscribe(
          response=>{
            console.log(response)
            for(let i=0;i<response.length;i++){
              var patientObj:Patient=new Patient();
              patientObj.Id=response[i]["id"];
              patientObj.PatientId=response[i]["patientId"];
              patientObj.FirstName=response[i]["firstName"];
              patientObj.LastName=response[i]["lastName"];
              patientObj.Gender=response[i]["gender"];
              patientObj.DateOfBirth=response[i]["dateOfBirth"];
              patientObj.PatientCreatedDate=response[i]["patientCreatedDate"];
              patientObj.ScanDetails=response[i]["scanDetails"]
              this.patientList.push(patientObj)
            }
    
            //console.log(this.patientList[0].FirstName)
          });
      }
    )

  }

  selectPatient(item: Patient) {

    this._IuDicomwebviewerService.getToken().subscribe(
      token=>{
        this._IuDicomwebviewerService.getScanDetailsbyPatientId(item.PatientId,token).subscribe(
          response=>{
            
            this.scanDetail.Id=response[0]['id']
            this.scanDetail.ImageModality=response[0]['imageModality']
            this.scanDetail.ImageType=response[0]['imageType']
            this.scanDetail.Patient=response[0]['patient']
            this.scanDetail.ScannedDate=response[0]['scannedDate']
            this.scanDetail.SeriesId=response[0]['seriesId']
            this.selectedStudyId=this.scanDetail.SeriesId.toString();
            this.patientId=this.scanDetail.Patient['patientId']
            this.fullName=this.scanDetail.Patient['firstName'] + " " + this.scanDetail.Patient['lastName']

            console.log(this.selectedStudyId)
            const data = { key: this.selectedStudyId};
            this._IuDicomwebviewerService.setStudyId(data);
            this.messageService.add({ severity: 'info', summary: 'Patient selected', detail: item.FirstName });
            this.router.navigate(['/patientdashboard/viewimages']);
          }
          
        )
      });

    

  }

  selectPatientforScanDetails(item:Patient){

    console.log(item.PatientId)

    this._IuDicomwebviewerService.getToken().subscribe(
      token=>{
        this._IuDicomwebviewerService.getScanDetailsbyPatientId(item.PatientId.toString(),token).subscribe(
          response=>{
            console.log(response)
            
            this.scanDetail.Id=response[0]['id']
            this.scanDetail.ImageModality=response[0]['imageModality']
            this.scanDetail.ImageType=response[0]['imageType']
            this.scanDetail.Patient=response[0]['patient']
    
            this.scanDetail.ScannedDate=response[0]['scannedDate']
            this.scanDetail.SeriesId=response[0]['seriesId']
            console.log(this.scanDetail.SeriesId);
            this.patientId=this.scanDetail.Patient['patientId']
            this.fullName=this.scanDetail.Patient['firstName'] + " " + this.scanDetail.Patient['lastName']
          }
        )


        this._IuDicomwebviewerService.getHospitalDetailsbyPatientId(item.PatientId.toString(),token).subscribe(
          result=>{
            console.log(result)
    
            this.institutionDetail.InstitutionName=result['institutionName']
            this.institutionDetail.DepartmentName=result['departmentName']
            this.institutionDetail.InstitutionAddress=result['institutionAddress']
            console.log(this.institutionDetail.InstitutionName)
          }
        )
      }
    )

    

    

    this.sidebarVisible=true;
  }

  getStudyId(PatientId:string){
    console.log('In get study');
    this._IuDicomwebviewerService.getToken().subscribe(
      token=>{
        this._IuDicomwebviewerService.getScanDetailsbyPatientId(PatientId,token).subscribe(
          response=>{
            
            this.scanDetail.Id=response[0]['id']
            this.scanDetail.ImageModality=response[0]['imageModality']
            this.scanDetail.ImageType=response[0]['imageType']
            this.scanDetail.Patient=response[0]['patient']
            this.scanDetail.ScannedDate=response[0]['scannedDate']
            this.scanDetail.SeriesId=response[0]['seriesId']
            this.selectedStudyId=this.scanDetail.SeriesId.toString();
            this.patientId=this.scanDetail.Patient['patientId']
            this.fullName=this.scanDetail.Patient['firstName'] + " " + this.scanDetail.Patient['lastName']
          }
        )
      });
  }



  logOut() {
    sessionStorage.removeItem('userName');
    sessionStorage.removeItem('userRole');
    sessionStorage.clear();
    this.router.navigate(['']);
  }

  homePage(){
    console.log(sessionStorage.getItem('userRole').toString())
    if(sessionStorage.getItem('userRole').toLowerCase()=="physician"){
      this.router.navigate(['/physician'])
    }
    else if(sessionStorage.getItem('userRole').toLowerCase()=="doctor"){
      this.router.navigate(['/doctor'])
    }
    else if(sessionStorage.getItem('userRole').toLowerCase()=="radiationoncologist"){
      this.router.navigate(['/radiologist'])
    }
    else if(sessionStorage.getItem('userRole').toLowerCase()=="admin"){
      this.router.navigate(['/admin'])
    }
 }
}
