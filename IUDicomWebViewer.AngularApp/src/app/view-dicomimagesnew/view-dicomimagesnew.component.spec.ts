import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewDicomimagesnewComponent } from './view-dicomimagesnew.component';

describe('ViewDicomimagesnewComponent', () => {
  let component: ViewDicomimagesnewComponent;
  let fixture: ComponentFixture<ViewDicomimagesnewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewDicomimagesnewComponent]
    });
    fixture = TestBed.createComponent(ViewDicomimagesnewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
