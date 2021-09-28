import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewdayTimelineComponent } from './newday-timeline.component';

describe('NewdayTimelineComponent', () => {
  let component: NewdayTimelineComponent;
  let fixture: ComponentFixture<NewdayTimelineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewdayTimelineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewdayTimelineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
