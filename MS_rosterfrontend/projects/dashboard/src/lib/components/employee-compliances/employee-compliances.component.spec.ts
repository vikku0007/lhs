import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeCompliancesComponent } from './employee-compliances.component';

describe('EmployeeCompliancesComponent', () => {
  let component: EmployeeCompliancesComponent;
  let fixture: ComponentFixture<EmployeeCompliancesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeCompliancesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeCompliancesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
