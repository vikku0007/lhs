import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeRosterComponent } from './employee-roster.component';

describe('EmployeeRosterComponent', () => {
  let component: EmployeeRosterComponent;
  let fixture: ComponentFixture<EmployeeRosterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeRosterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeRosterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
