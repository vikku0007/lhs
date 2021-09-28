import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignedshiftComponent } from './assignedshift.component';

describe('AssignedshiftComponent', () => {
  let component: AssignedshiftComponent;
  let fixture: ComponentFixture<AssignedshiftComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignedshiftComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignedshiftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
