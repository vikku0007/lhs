import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgressShiftListComponent } from './progress-shift-list.component';

describe('ProgressShiftListComponent', () => {
  let component: ProgressShiftListComponent;
  let fixture: ComponentFixture<ProgressShiftListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgressShiftListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgressShiftListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
