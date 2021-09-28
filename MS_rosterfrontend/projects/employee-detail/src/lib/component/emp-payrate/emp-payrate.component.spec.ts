import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpPayrateComponent } from './emp-payrate.component';

describe('EmpPayrateComponent', () => {
  let component: EmpPayrateComponent;
  let fixture: ComponentFixture<EmpPayrateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpPayrateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpPayrateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
