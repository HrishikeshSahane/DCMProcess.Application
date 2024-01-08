import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-radiologist-layout',
  templateUrl: './radiologist-layout.component.html',
  styleUrls: ['./radiologist-layout.component.css']
})
export class RadiologistLayoutComponent {
  constructor(private router: Router) { }
  ngOnInit() {
  }
  logOut() {
    sessionStorage.removeItem('userName');
    sessionStorage.removeItem('userRole');
    sessionStorage.clear();
    this.router.navigate(['']);
  }

  viewpatients(){
    console.log("IN view patients method")
    this.router.navigate(['/patientdashboard']);
  }
}
