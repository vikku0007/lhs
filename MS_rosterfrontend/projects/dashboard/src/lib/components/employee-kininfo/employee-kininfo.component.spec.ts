import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeKinInfoComponent } from './employee-kininfo.component';

describe('EmployeekininfoComponent', () => {
  let component: EmployeeKinInfoComponent;
  let fixture: ComponentFixture<EmployeeKinInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeKinInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeKinInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
