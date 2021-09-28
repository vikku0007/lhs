import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeDrivingLicenseInfoComponent } from './employee-miscinfo.component';

describe('EmployeekininfoComponent', () => {
  let component: EmployeeDrivingLicenseInfoComponent;
  let fixture: ComponentFixture<EmployeeDrivingLicenseInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDrivingLicenseInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDrivingLicenseInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
