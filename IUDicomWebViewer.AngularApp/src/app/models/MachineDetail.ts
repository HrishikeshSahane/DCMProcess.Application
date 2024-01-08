import { IMachineDetail } from "../interfaces/IMachineDetail";

export class MachineDetail implements IMachineDetail{
    Id: number;
    SeriesId: string;
    PatientId: string;
    ImageModality: string;
    ImageType: string;
    PatientPosition: string;
    PatientOrientation: string;
    ScannedDate: Date;
    ModifiedDate: Date;

}