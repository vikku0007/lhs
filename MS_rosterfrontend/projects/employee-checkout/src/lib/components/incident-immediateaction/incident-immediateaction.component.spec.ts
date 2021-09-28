import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentImmediateactionComponent } from './incident-immediateaction.component';

describe('IncidentImmediateactionComponent', () => {
  let component: IncidentImmediateactionComponent;
  let fixture: ComponentFixture<IncidentImmediateactionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentImmediateactionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentImmediateactionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
