import { IUsers } from "../interfaces/IUsers";
import {Role} from "../models/Role"

export class Users implements IUsers{
    FirstName: string='';
    LastName: string='';
    EmailId: string='';
    Gender:string='';
    UserPassword: string='';
    Speciality:string='';
    CurrentWorkPlace:string='';
    RoleId: number=0;
    DateOfBirth: Date=new Date();
    Role:Role
}