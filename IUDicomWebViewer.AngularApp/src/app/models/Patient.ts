import { IPatient } from "../interfaces/IPatient";
import { ScanDetail } from "./ScanDetail";

export class Patient implements IPatient{
    Id: number=0;
    PatientId: string='';
    FirstName: string='';
    LastName: string='';
    Gender: string='';
    DateOfBirth: Date=new Date();
    PatientCreatedDate:Date= new Date();
    ScanDetails: ScanDetail[]=[]
}
