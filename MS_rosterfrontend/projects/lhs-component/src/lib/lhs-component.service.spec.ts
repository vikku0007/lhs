import { TestBed } from '@angular/core/testing';

import { LhsComponentService } from './lhs-component.service';

describe('LhsComponentService', () => {
  let service: LhsComponentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LhsComponentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
