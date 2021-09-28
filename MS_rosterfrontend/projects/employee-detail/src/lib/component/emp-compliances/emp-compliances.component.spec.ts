import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpCompliancesComponent } from './emp-compliances.component';

describe('EmpCompliancesComponent', () => {
  let component: EmpCompliancesComponent;
  let fixture: ComponentFixture<EmpCompliancesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpCompliancesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpCompliancesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
