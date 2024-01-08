import { IRole } from "../interfaces/IRole";
import { Users } from "./Users";

export class Role implements IRole{
    RoleId: number;
    RoleName: string;
    Users: Users[];
    
}