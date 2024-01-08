import { Users } from "../models/Users";

export interface IRole{
    RoleId:number;
    RoleName:string;
    Users:Users[]
}