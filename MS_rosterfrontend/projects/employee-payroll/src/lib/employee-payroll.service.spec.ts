import { TestBed } from '@angular/core/testing';

import { EmployeePayrollService } from './employee-payroll.service';

describe('EmployeePayrollService', () => {
  let service: EmployeePayrollService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmployeePayrollService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
