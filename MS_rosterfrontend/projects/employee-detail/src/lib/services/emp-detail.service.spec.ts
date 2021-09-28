import { TestBed } from '@angular/core/testing';

import { EmpDetailService } from './emp-detail.service';

describe('EmpDetailService', () => {
  let service: EmpDetailService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmpDetailService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
