import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeAwardsComponent } from './employee-awards.component';

describe('EmployeeAwardsComponent', () => {
  let component: EmployeeAwardsComponent;
  let fixture: ComponentFixture<EmployeeAwardsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeAwardsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeAwardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
