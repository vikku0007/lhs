import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeePayRateseComponent } from './employee-payrates.component';

describe('EmployeekininfoComponent', () => {
  let component: EmployeePayRateseComponent;
  let fixture: ComponentFixture<EmployeePayRateseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeePayRateseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeePayRateseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
