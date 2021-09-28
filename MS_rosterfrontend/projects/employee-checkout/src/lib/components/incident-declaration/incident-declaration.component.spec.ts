import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentDeclarationComponent } from './incident-declaration.component';

describe('IncidentDeclarationComponent', () => {
  let component: IncidentDeclarationComponent;
  let fixture: ComponentFixture<IncidentDeclarationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentDeclarationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentDeclarationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
