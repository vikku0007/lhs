import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayrollInMyobComponent } from './payroll-in-myob.component';

describe('PayrollInMyobComponent', () => {
  let component: PayrollInMyobComponent;
  let fixture: ComponentFixture<PayrollInMyobComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PayrollInMyobComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayrollInMyobComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
