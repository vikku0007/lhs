import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpNextKinComponent } from './emp-next-kin.component';

describe('EmpNextKinComponent', () => {
  let component: EmpNextKinComponent;
  let fixture: ComponentFixture<EmpNextKinComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpNextKinComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpNextKinComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
