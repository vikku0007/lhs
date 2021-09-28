import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpJobProfileComponent } from './emp-job-profile.component';

describe('EmpJobProfileComponent', () => {
  let component: EmpJobProfileComponent;
  let fixture: ComponentFixture<EmpJobProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpJobProfileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpJobProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
