import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentReportedNewComponent } from './incident-reported-new.component';

describe('IncidentReportedNewComponent', () => {
  let component: IncidentReportedNewComponent;
  let fixture: ComponentFixture<IncidentReportedNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentReportedNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentReportedNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
