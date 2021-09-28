import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HourlyTimesheetComponent } from './hourly-timesheet.component';

describe('HourlyTimesheetComponent', () => {
  let component: HourlyTimesheetComponent;
  let fixture: ComponentFixture<HourlyTimesheetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HourlyTimesheetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HourlyTimesheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
