import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentRiskassesmentComponent } from './incident-riskassesment.component';

describe('IncidentRiskassesmentComponent', () => {
  let component: IncidentRiskassesmentComponent;
  let fixture: ComponentFixture<IncidentRiskassesmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentRiskassesmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentRiskassesmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
