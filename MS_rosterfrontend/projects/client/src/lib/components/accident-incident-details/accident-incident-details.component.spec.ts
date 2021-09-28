import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccidentIncidentDetailsComponent } from './accident-incident-details.component';

describe('AccidentIncidentDetailsComponent', () => {
  let component: AccidentIncidentDetailsComponent;
  let fixture: ComponentFixture<AccidentIncidentDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccidentIncidentDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccidentIncidentDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
