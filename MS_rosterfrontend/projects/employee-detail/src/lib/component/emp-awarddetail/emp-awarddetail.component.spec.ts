import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpAwarddetailComponent } from './emp-awarddetail.component';

describe('EmpAwarddetailComponent', () => {
  let component: EmpAwarddetailComponent;
  let fixture: ComponentFixture<EmpAwarddetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpAwarddetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpAwarddetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
