import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RadiologistLayoutComponent } from './radiologist-layout.component';

describe('RadiologistLayoutComponent', () => {
  let component: RadiologistLayoutComponent;
  let fixture: ComponentFixture<RadiologistLayoutComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RadiologistLayoutComponent]
    });
    fixture = TestBed.createComponent(RadiologistLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
