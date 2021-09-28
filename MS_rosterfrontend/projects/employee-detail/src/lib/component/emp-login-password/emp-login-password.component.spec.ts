import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpLoginPasswordComponent } from './emp-login-password.component';

describe('EmpLoginPasswordComponent', () => {
  let component: EmpLoginPasswordComponent;
  let fixture: ComponentFixture<EmpLoginPasswordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpLoginPasswordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpLoginPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
