import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpAccidentIncidentComponent } from './emp-accident-incident.component';

describe('EmpAccidentIncidentComponent', () => {
  let component: EmpAccidentIncidentComponent;
  let fixture: ComponentFixture<EmpAccidentIncidentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpAccidentIncidentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpAccidentIncidentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
