import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentshiftComponent } from './currentshift.component';

describe('CurrentshiftComponent', () => {
  let component: CurrentshiftComponent;
  let fixture: ComponentFixture<CurrentshiftComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CurrentshiftComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrentshiftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
