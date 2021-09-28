import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccidentIncidentListComponent } from './accident-incident-list.component';

describe('AccidentIncidentListComponent', () => {
  let component: AccidentIncidentListComponent;
  let fixture: ComponentFixture<AccidentIncidentListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccidentIncidentListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccidentIncidentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
