import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { WebImageViewerComponent } from './webimage-viewer.component';
import { CornerstoneDirective } from './cornerstone.directive';
import { ThumbnailDirective } from './thumbnail.directive';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    MatProgressSpinnerModule,
    FontAwesomeModule
  ],
  declarations: [WebImageViewerComponent, CornerstoneDirective, ThumbnailDirective],
  exports: [WebImageViewerComponent, CornerstoneDirective, ThumbnailDirective]
})
export class DicomViewerModule { }
