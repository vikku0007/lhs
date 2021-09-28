import { TestBed } from '@angular/core/testing';

import { EmployeeCheckoutService } from './employee-checkout.service';

describe('EmployeeCheckoutService', () => {
  let service: EmployeeCheckoutService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmployeeCheckoutService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
