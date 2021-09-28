import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgressNotesDetailsComponent } from './progress-notes-details.component';

describe('ProgressNotesDetailsComponent', () => {
  let component: ProgressNotesDetailsComponent;
  let fixture: ComponentFixture<ProgressNotesDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgressNotesDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgressNotesDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
