import { MachineDetail } from "../models/MachineDetail";
import { Patient } from "../models/Patient";

export interface IScanDetail {
    Id: number;
    SeriesId: string;
    PatientId: string;
    ImageModality:string;
    ImageType: string;
    PatientPosition:string;
    PatientOrientation:string;
    ScannedDate:Date;
    ModifiedDate:Date;
    MachineDetail:MachineDetail[];
    Patient:Patient;
  }
  