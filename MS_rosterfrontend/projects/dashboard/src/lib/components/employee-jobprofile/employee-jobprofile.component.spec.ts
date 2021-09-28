import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeJobProfileComponent } from './employee-jobprofile.component';

describe('EmployeekininfoComponent', () => {
  let component: EmployeeJobProfileComponent;
  let fixture: ComponentFixture<EmployeeJobProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeJobProfileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeJobProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
