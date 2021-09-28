import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WeekTimelineComponent } from './week-timeline.component';

describe('WeekTimelineComponent', () => {
  let component: WeekTimelineComponent;
  let fixture: ComponentFixture<WeekTimelineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WeekTimelineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WeekTimelineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
