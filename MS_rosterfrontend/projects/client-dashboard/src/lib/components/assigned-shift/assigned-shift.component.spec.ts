import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignedShiftComponent } from './assigned-shift.component';

describe('AssignedShiftComponent', () => {
  let component: AssignedShiftComponent;
  let fixture: ComponentFixture<AssignedShiftComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignedShiftComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignedShiftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
