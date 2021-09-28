import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeOthersComponent } from './employee-others.component';

describe('EmployeeOthersComponent', () => {
  let component: EmployeeOthersComponent;
  let fixture: ComponentFixture<EmployeeOthersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeOthersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeOthersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
