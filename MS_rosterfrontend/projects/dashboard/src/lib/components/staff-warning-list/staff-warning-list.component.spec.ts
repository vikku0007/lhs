import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffWarningListComponent } from './staff-warning-list.component';

describe('StaffWarningListComponent', () => {
  let component: StaffWarningListComponent;
  let fixture: ComponentFixture<StaffWarningListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StaffWarningListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StaffWarningListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
