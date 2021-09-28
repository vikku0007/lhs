import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeLoginPasswordComponent } from './employee-login-password.component';

describe('EmployeeLoginPasswordComponent', () => {
  let component: EmployeeLoginPasswordComponent;
  let fixture: ComponentFixture<EmployeeLoginPasswordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeLoginPasswordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeLoginPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
