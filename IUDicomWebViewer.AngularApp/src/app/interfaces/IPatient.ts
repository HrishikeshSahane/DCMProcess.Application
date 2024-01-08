import { ScanDetail } from "../models/ScanDetail";

export interface IPatient {
    Id: number;
    PatientId: string;
    FirstName: string;
    LastName:string;
    Gender: string;
    DateOfBirth:Date;
    PatientCreatedDate:Date;
    ScanDetails:ScanDetail[];
  }
  

  export interface IPatientModel {
    Id: number;
    PatientId: string;
    FirstName: string;
    LastName:string;
    Gender: string;
    DateOfBirth:Date;
    PatientCreatedDate:Date;
  }