import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComplianceListComponent } from './compliance-list.component';

describe('ComplianceListComponent', () => {
  let component: ComplianceListComponent;
  let fixture: ComponentFixture<ComplianceListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComplianceListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComplianceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
