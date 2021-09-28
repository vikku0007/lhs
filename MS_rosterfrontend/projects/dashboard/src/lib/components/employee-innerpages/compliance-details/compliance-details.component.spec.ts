import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComplianceDetailsComponent } from './compliance-details.component';

describe('ComplianceDetailsComponent', () => {
  let component: ComplianceDetailsComponent;
  let fixture: ComponentFixture<ComplianceDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComplianceDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComplianceDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
