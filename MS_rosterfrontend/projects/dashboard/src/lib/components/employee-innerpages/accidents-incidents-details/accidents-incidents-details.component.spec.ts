import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccidentsIncidentsDetailsComponent } from './accidents-incidents-details.component';

describe('AccidentsIncidentsDetailsComponent', () => {
  let component: AccidentsIncidentsDetailsComponent;
  let fixture: ComponentFixture<AccidentsIncidentsDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccidentsIncidentsDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccidentsIncidentsDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
