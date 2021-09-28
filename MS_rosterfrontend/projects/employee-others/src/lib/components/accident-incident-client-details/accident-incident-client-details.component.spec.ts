import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccidentIncidentClientDetailsComponent } from './accident-incident-client-details.component';

describe('AccidentIncidentClientDetailsComponent', () => {
  let component: AccidentIncidentClientDetailsComponent;
  let fixture: ComponentFixture<AccidentIncidentClientDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccidentIncidentClientDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccidentIncidentClientDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
