import { IInstitutionDetail } from "../interfaces/IInstitutionDetail";

export class InstitutionDetail implements IInstitutionDetail{
    Id: number;
    InstitutionName: string;
    DepartmentName: string;
    InstitutionAddress: string;
    SeriesId: string;
    CreatedDate: Date;

}