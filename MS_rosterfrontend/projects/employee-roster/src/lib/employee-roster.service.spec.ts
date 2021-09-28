import { TestBed } from '@angular/core/testing';

import { EmployeeRosterService } from './employee-roster.service';

describe('EmployeeRosterService', () => {
  let service: EmployeeRosterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmployeeRosterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
