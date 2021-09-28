import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LeaveApprovedComponent } from './leave-approved.component';

describe('LeaveApprovedComponent', () => {
  let component: LeaveApprovedComponent;
  let fixture: ComponentFixture<LeaveApprovedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LeaveApprovedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LeaveApprovedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
