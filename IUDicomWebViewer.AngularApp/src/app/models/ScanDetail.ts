import { IScanDetail } from "../interfaces/IScanDetail";
import { MachineDetail } from "./MachineDetail";
import { Patient } from "./Patient";

export class ScanDetail implements IScanDetail{
    Id: number;
    SeriesId: string;
    PatientId: string;
    ImageModality: string;
    ImageType: string;
    PatientPosition: string;
    PatientOrientation: string;
    ScannedDate: Date=new Date();
    ModifiedDate: Date=new Date();
    MachineDetail: MachineDetail[]=[];
    Patient: Patient=null!;

}