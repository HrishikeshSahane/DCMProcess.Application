import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewDicomimagesComponent } from './view-dicomimages.component';

describe('ViewDicomimagesComponent', () => {
  let component: ViewDicomimagesComponent;
  let fixture: ComponentFixture<ViewDicomimagesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewDicomimagesComponent]
    });
    fixture = TestBed.createComponent(ViewDicomimagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
