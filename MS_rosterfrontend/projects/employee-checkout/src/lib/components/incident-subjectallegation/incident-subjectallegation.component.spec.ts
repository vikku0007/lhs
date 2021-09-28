import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentSubjectallegationComponent } from './incident-subjectallegation.component';

describe('IncidentSubjectallegationComponent', () => {
  let component: IncidentSubjectallegationComponent;
  let fixture: ComponentFixture<IncidentSubjectallegationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentSubjectallegationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentSubjectallegationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
