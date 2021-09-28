import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgressNotesListComponent } from './progress-notes-list.component';

describe('ProgressNotesListComponent', () => {
  let component: ProgressNotesListComponent;
  let fixture: ComponentFixture<ProgressNotesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgressNotesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgressNotesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
