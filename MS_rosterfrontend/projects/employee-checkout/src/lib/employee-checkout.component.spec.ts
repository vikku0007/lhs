import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeCheckoutComponent } from './employee-checkout.component';

describe('EmployeeCheckoutComponent', () => {
  let component: EmployeeCheckoutComponent;
  let fixture: ComponentFixture<EmployeeCheckoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeCheckoutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeCheckoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
