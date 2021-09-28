import { TestBed } from '@angular/core/testing';

import { LhsServiceService } from './lhs-service.service';

describe('LhsServiceService', () => {
  let service: LhsServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LhsServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
