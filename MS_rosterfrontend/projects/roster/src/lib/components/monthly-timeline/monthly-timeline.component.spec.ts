import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthlyTimelineComponent } from './monthly-timeline.component';

describe('MonthlyTimelineComponent', () => {
  let component: MonthlyTimelineComponent;
  let fixture: ComponentFixture<MonthlyTimelineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MonthlyTimelineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MonthlyTimelineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
