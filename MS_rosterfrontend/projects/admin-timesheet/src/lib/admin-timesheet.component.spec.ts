import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTimesheetComponent } from './admin-timesheet.component';

describe('AdminTimesheetComponent', () => {
  let component: AdminTimesheetComponent;
  let fixture: ComponentFixture<AdminTimesheetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminTimesheetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTimesheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
