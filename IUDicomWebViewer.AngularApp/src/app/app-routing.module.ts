import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomeComponent} from './home/home.component'
import { LoginComponent } from './login/login.component';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { DoctorLayoutComponent } from './doctor-layout/doctor-layout.component';
import { RadiologistLayoutComponent } from './radiologist-layout/radiologist-layout.component';
import { PhysicianLayoutComponent } from './physician-layout/physician-layout.component';
import { PatientDashboardComponent } from './patient-dashboard/patient-dashboard.component';
import { ViewDicomimagesComponent } from './view-dicomimages/view-dicomimages.component';
import { RegisterUserComponent } from './register-user/register-user.component';

const routes: Routes = [
    { path: 'home', component: HomeComponent },
    { path: 'login', component: LoginComponent },
    { path: 'admin', component: AdminLayoutComponent },
    { path: 'doctor', component: DoctorLayoutComponent },
    { path: 'physician', component: PhysicianLayoutComponent },
    { path: 'radiologist', component: RadiologistLayoutComponent },
    { path: 'patientdashboard', component: PatientDashboardComponent },
    { path: 'patientdashboard/viewimages', component: ViewDicomimagesComponent },
    { path: 'registeruser', component: RegisterUserComponent },
    //default
    { path: '**', component: LoginComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }



