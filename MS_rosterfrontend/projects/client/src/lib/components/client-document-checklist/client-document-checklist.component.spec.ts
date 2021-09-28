import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientDocumentChecklistComponent } from './client-document-checklist.component';

describe('ClientDocumentChecklistComponent', () => {
  let component: ClientDocumentChecklistComponent;
  let fixture: ComponentFixture<ClientDocumentChecklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientDocumentChecklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientDocumentChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
