import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeTodayShiftComponent } from './employee-today-shift.component';

describe('EmployeeTodayShiftComponent', () => {
  let component: EmployeeTodayShiftComponent;
  let fixture: ComponentFixture<EmployeeTodayShiftComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeTodayShiftComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeTodayShiftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
