import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpDriverlicenseComponent } from './emp-driverlicense.component';

describe('EmpDriverlicenseComponent', () => {
  let component: EmpDriverlicenseComponent;
  let fixture: ComponentFixture<EmpDriverlicenseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpDriverlicenseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpDriverlicenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
