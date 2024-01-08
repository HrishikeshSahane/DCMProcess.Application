import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WebImageViewerComponent } from './webimage-viewer.component';

describe('DICOMViewerComponent', () => {
  let component: WebImageViewerComponent;
  let fixture: ComponentFixture<WebImageViewerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WebImageViewerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WebImageViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
