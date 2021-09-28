import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentContactpersonComponent } from './incident-contactperson.component';

describe('IncidentContactpersonComponent', () => {
  let component: IncidentContactpersonComponent;
  let fixture: ComponentFixture<IncidentContactpersonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentContactpersonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentContactpersonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
