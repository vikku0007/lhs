import { TestBed } from '@angular/core/testing';

import { LhsDirectivesService } from './lhs-directives.service';

describe('LhsDirectivesService', () => {
  let service: LhsDirectivesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LhsDirectivesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
