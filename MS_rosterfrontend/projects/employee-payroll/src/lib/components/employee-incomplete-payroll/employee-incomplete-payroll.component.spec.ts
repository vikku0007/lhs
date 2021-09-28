import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeIncompletePayrollComponent } from './employee-incomplete-payroll.component';

describe('EmployeeIncompletePayrollComponent', () => {
  let component: EmployeeIncompletePayrollComponent;
  let fixture: ComponentFixture<EmployeeIncompletePayrollComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeIncompletePayrollComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeIncompletePayrollComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
