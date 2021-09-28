import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpTodayShiftComponent } from './emp-today-shift.component';

describe('EmpTodayShiftComponent', () => {
  let component: EmpTodayShiftComponent;
  let fixture: ComponentFixture<EmpTodayShiftComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpTodayShiftComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpTodayShiftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
