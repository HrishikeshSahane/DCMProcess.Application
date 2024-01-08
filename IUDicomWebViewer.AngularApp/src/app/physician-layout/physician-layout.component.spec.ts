import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhysicianLayoutComponent } from './physician-layout.component';

describe('PhysicianLayoutComponent', () => {
  let component: PhysicianLayoutComponent;
  let fixture: ComponentFixture<PhysicianLayoutComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PhysicianLayoutComponent]
    });
    fixture = TestBed.createComponent(PhysicianLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
