import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import {IuDicomwebviewerService} from '../services/iu-dicomwebviewer.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  status: string;
  msg: string;
  showDiv: boolean = false;
  ngOnInit(): void {}
  constructor(private _IuDicomwebviewerService: IuDicomwebviewerService,private router:Router){

  }
  submitLoginForm(form: NgForm) {
    var token:string=""
    console.log("In Login Component");
    this._IuDicomwebviewerService.getToken().subscribe(
      result=>{
        this._IuDicomwebviewerService.validateCredentials(form.value.email, form.value.password,result).subscribe(
          response=>{
            this.status=response;
            if (this.status.toLowerCase() != "invalid credentials") {
              sessionStorage.setItem('userName', form.value.email);
              sessionStorage.setItem('userRole', this.status.toLowerCase());
              console.log(response);
    
              if(this.status.toLowerCase()=="admin"){
                this.router.navigate(['/admin']);
              }
              else if(this.status.toLowerCase()=="doctor"){
                this.router.navigate(['/doctor']);
              }
    
              else if(this.status.toLowerCase()=="physician"){
                this.router.navigate(['/physician']);
              }
    
              else if(this.status.toLowerCase()=="radiationoncologist"){
                this.router.navigate(['/radiologist']);
              }
    
            }
            else {
              this.msg = this.status + ". Try again with valid credentials.";
              this.showDiv = true;
              
            }
          });
      }
    )


  }
  

  showmessage(): void {
    alert("Kindly contact Customer Care for password reset.Reach out to customercare@iubh.com");
  }
}
