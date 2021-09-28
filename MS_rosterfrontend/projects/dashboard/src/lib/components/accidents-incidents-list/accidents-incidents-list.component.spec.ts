import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccidentsIncidentsListComponent } from './accidents-incidents-list.component';

describe('AccidentsIncidentsListComponent', () => {
  let component: AccidentsIncidentsListComponent;
  let fixture: ComponentFixture<AccidentsIncidentsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccidentsIncidentsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccidentsIncidentsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
