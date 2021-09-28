import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpStaffWarningComponent } from './emp-staff-warning.component';

describe('EmpStaffWarningComponent', () => {
  let component: EmpStaffWarningComponent;
  let fixture: ComponentFixture<EmpStaffWarningComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpStaffWarningComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpStaffWarningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
