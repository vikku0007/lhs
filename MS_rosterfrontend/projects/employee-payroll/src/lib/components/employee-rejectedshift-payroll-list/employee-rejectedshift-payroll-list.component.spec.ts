import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeRejectedshiftPayrollListComponent } from './employee-rejectedshift-payroll-list.component';

describe('EmployeeRejectedshiftPayrollListComponent', () => {
  let component: EmployeeRejectedshiftPayrollListComponent;
  let fixture: ComponentFixture<EmployeeRejectedshiftPayrollListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeRejectedshiftPayrollListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeRejectedshiftPayrollListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
