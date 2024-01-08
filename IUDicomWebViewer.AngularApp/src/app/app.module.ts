import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { DicomViewerModule } from '../../projects/dicom-viewer/src/lib/webimage-viewer.module';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component'
import { IuDicomwebviewerService } from './services/iu-dicomwebviewer.service';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { DoctorLayoutComponent } from './doctor-layout/doctor-layout.component';
import { RadiologistLayoutComponent } from './radiologist-layout/radiologist-layout.component';
import { PhysicianLayoutComponent } from './physician-layout/physician-layout.component';
import { PatientDashboardComponent } from './patient-dashboard/patient-dashboard.component';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { SidebarModule } from 'primeng/sidebar';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCoffee } from '@fortawesome/free-solid-svg-icons';
import { ViewDicomimagesComponent } from './view-dicomimages/view-dicomimages.component';
import { ViewDicomimagesnewComponent } from './view-dicomimagesnew/view-dicomimagesnew.component';
import { RegisterUserComponent } from './register-user/register-user.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    AdminLayoutComponent,
    DoctorLayoutComponent,
    RadiologistLayoutComponent,
    PhysicianLayoutComponent,
    PatientDashboardComponent,
    ViewDicomimagesComponent,
    ViewDicomimagesnewComponent,
    RegisterUserComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    FontAwesomeModule,
    AppRoutingModule,
    CommonModule,
    MatProgressSpinnerModule,
    DicomViewerModule,
    HttpClientModule,
    BrowserAnimationsModule,
    TableModule,
    ToastModule,
    SidebarModule
  ],
  providers: [IuDicomwebviewerService],
  bootstrap: [AppComponent]
})
export class AppModule { }
