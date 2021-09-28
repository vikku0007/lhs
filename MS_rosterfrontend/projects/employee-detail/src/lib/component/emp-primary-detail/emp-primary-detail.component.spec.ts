import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpPrimaryDetailComponent } from './emp-primary-detail.component';

describe('EmpPrimaryDetailComponent', () => {
  let component: EmpPrimaryDetailComponent;
  let fixture: ComponentFixture<EmpPrimaryDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpPrimaryDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpPrimaryDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
