import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeDrivinglicenseComponent } from './employee-drivinglicense.component';

describe('EmployeeDrivinglicenseComponent', () => {
  let component: EmployeeDrivinglicenseComponent;
  let fixture: ComponentFixture<EmployeeDrivinglicenseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDrivinglicenseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDrivinglicenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
