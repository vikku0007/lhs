import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppraisalListComponent } from './appraisal-list.component';

describe('AppraisalListComponent', () => {
  let component: AppraisalListComponent;
  let fixture: ComponentFixture<AppraisalListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppraisalListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppraisalListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
