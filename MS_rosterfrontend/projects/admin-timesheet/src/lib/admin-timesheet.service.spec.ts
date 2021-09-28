import { TestBed } from '@angular/core/testing';

import { AdminTimesheetService } from './admin-timesheet.service';

describe('AdminTimesheetService', () => {
  let service: AdminTimesheetService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminTimesheetService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
