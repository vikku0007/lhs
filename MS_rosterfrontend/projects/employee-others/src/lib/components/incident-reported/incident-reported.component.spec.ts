import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentReportedComponent } from './incident-reported.component';

describe('IncidentReportedComponent', () => {
  let component: IncidentReportedComponent;
  let fixture: ComponentFixture<IncidentReportedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentReportedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentReportedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
