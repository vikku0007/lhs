import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentImpactedpersonComponent } from './incident-impactedperson.component';

describe('IncidentImpactedpersonComponent', () => {
  let component: IncidentImpactedpersonComponent;
  let fixture: ComponentFixture<IncidentImpactedpersonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentImpactedpersonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentImpactedpersonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
