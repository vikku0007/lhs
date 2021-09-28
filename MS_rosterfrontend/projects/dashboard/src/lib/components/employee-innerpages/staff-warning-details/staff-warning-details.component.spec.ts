import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffWarningDetailsComponent } from './staff-warning-details.component';

describe('StaffWarningDetailsComponent', () => {
  let component: StaffWarningDetailsComponent;
  let fixture: ComponentFixture<StaffWarningDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StaffWarningDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StaffWarningDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
