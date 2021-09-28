import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAdditionalNotesComponent } from './client-additional-notes.component';

describe('ClientAdditionalNotesComponent', () => {
  let component: ClientAdditionalNotesComponent;
  let fixture: ComponentFixture<ClientAdditionalNotesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAdditionalNotesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAdditionalNotesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
