import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpQualificationComponent } from './emp-qualification.component';

describe('EmpQualificationComponent', () => {
  let component: EmpQualificationComponent;
  let fixture: ComponentFixture<EmpQualificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpQualificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpQualificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
