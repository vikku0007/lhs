import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LhsDirectivesComponent } from './lhs-directives.component';

describe('LhsDirectivesComponent', () => {
  let component: LhsDirectivesComponent;
  let fixture: ComponentFixture<LhsDirectivesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LhsDirectivesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LhsDirectivesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
