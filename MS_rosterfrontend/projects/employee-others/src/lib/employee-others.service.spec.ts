import { TestBed } from '@angular/core/testing';

import { EmployeeOthersService } from './employee-others.service';

describe('EmployeeOthersService', () => {
  let service: EmployeeOthersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmployeeOthersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
