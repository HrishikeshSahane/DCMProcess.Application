import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Users } from '../models/Users';
import { IuDicomwebviewerService } from '../services/iu-dicomwebviewer.service';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent {
  issuccess:boolean=true;
  password:string;
  password2: string;

  status: string;
  errorMsg: string;
  msg: string;
  showDiv: boolean = false;
  num: number;
  constructor(private _IuDicomwebviewerService: IuDicomwebviewerService,private router:Router) { }

  ngOnInit() {
  }


  RegistrationForm(form:NgForm)
  {
    var registerUser=new Users();
    registerUser.FirstName=form.value.firstname
    registerUser.LastName=form.value.lastname
    registerUser.EmailId=form.value.email
    registerUser.Gender=(form.value.gender).toLowerCase()
    if(this.password==this.password2){
      registerUser.UserPassword=form.value.password
    }
    registerUser.Speciality=form.value.specilaity
    if(form.value.roleName.toLowerCase()=="physician"){
      registerUser.RoleId=3
    }
    else if (form.value.roleName.toLowerCase()=="radiologist"){
      registerUser.RoleId=2
    }
    else if(form.value.roleName.toLowerCase()=="doctor"){
      registerUser.RoleId=1
    }
    else{
      this.issuccess=false;
    }
    registerUser.DateOfBirth=form.value.dateOfBirth
    registerUser.Speciality=form.value.speciality
    registerUser.CurrentWorkPlace=form.value.currentworkplace
    console.log(registerUser);


    this._IuDicomwebviewerService.getToken().subscribe(
      token=>{
        this._IuDicomwebviewerService.registerUser(registerUser,token).subscribe(
          response=>{
            console.log(response)
          }
        )
      }
    )


    
  }  

  logOut(){
    sessionStorage.removeItem('userName');
    sessionStorage.removeItem('userRole');
    sessionStorage.clear();
    this.router.navigate(['']);
  }

  homePage(){
    console.log(sessionStorage.getItem('userRole').toString())
    if(sessionStorage.getItem('userRole').toLowerCase()=="physician"){
      this.router.navigate(['/physician'])
    }
    else if(sessionStorage.getItem('userRole').toLowerCase()=="doctor"){
      this.router.navigate(['/doctor'])
    }
    else if(sessionStorage.getItem('userRole').toLowerCase()=="radiationoncologist"){
      this.router.navigate(['/radiologist'])
    }
    else if(sessionStorage.getItem('userRole').toLowerCase()=="admin"){
      this.router.navigate(['/admin'])
    }
  }
}
