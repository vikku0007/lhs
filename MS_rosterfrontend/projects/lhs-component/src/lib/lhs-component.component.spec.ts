import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LhsComponentComponent } from './lhs-component.component';

describe('LhsComponentComponent', () => {
  let component: LhsComponentComponent;
  let fixture: ComponentFixture<LhsComponentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LhsComponentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LhsComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
