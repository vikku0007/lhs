import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentProviderdetailComponent } from './incident-providerdetail.component';

describe('IncidentProviderdetailComponent', () => {
  let component: IncidentProviderdetailComponent;
  let fixture: ComponentFixture<IncidentProviderdetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentProviderdetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentProviderdetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
