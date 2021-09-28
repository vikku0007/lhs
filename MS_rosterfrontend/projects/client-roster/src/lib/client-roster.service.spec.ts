import { TestBed } from '@angular/core/testing';

import { ClientRosterService } from './client-roster.service';

describe('ClientRosterService', () => {
  let service: ClientRosterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientRosterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
