import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientRosterComponent } from './client-roster.component';

describe('ClientRosterComponent', () => {
  let component: ClientRosterComponent;
  let fixture: ComponentFixture<ClientRosterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientRosterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientRosterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
