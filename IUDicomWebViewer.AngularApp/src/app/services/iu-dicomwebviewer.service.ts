import { Injectable } from '@angular/core';
import {environment} from '../../environments/environment'
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import {Users} from '../models/Users';
import {Patient} from '../models/Patient';
import { IPatient } from '../interfaces/IPatient';
import { ScanDetail } from '../models/ScanDetail';
import { InstitutionDetail } from '../models/InstitutionDetail';
import { JWTClientDetails } from '../models/JWTClientDetails';

@Injectable({
  providedIn: 'root'
})
export class IuDicomwebviewerService {

  private dataSubject = new BehaviorSubject<any>(null);
  public data$ = this.dataSubject.asObservable();
  constructor(private _httpclient: HttpClient) { 
  }

  getToken(){
    
    var jwtclientDetails= new JWTClientDetails()
    jwtclientDetails.ClientID=environment.clientId,
    jwtclientDetails.ClientSecret=environment.clientSecret
    return this._httpclient.post<string>(environment.baseURL+"security/createToken",jwtclientDetails);
  }


  validateCredentials(emailId: string, password: string,tokenvalue:string): Observable<string> {
   var userObj:Users=new Users()
   userObj.EmailId=emailId;
   userObj.UserPassword=password
   console.log(userObj);
   const headers = new HttpHeaders({
    'Authorization': `Bearer ${tokenvalue}`
  });

   return this._httpclient.post<string>(environment.baseURL+"User/Login",userObj,{headers});

  }


  registerUser(registerUser:Users,tokenvalue:string):Observable<string>{
    console.log(tokenvalue)
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${tokenvalue}`
    });
    return this._httpclient.post<string>(environment.baseURL+"User/Register",registerUser,{headers});
  }

  getAllPatients(tokenvalue:string):Observable<Patient[]>{
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${tokenvalue}`
    });
    return this._httpclient.get<Patient[]>(environment.baseURL+"Patient/GetPatientList",{headers})
  }

  getScanDetailsbyPatientId(patientId:string,tokenvalue:string):Observable<ScanDetail>{
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${tokenvalue}`
    });
    return this._httpclient.get<ScanDetail>(environment.baseURL+"Patient/GetScanDetails?patientId="+patientId,{headers})
  }

  getHospitalDetailsbyPatientId(patientId:string,tokenvalue:string):Observable<InstitutionDetail>{
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${tokenvalue}`
    });
    return this._httpclient.get<InstitutionDetail>(environment.baseURL+"Institute/GetInstitutionDetails?patientId="+patientId,{headers})
  }

  // getBlobSasToken():Observable<string>{
  //   return this._httpclient.get<string>(environment.baseURL+"Storage/GetBlobSasToken")
  // }

  setStudyId(data:any){
    console.log('Setting StudyId ' + data.key);
    this.dataSubject.next(data);
  }
}