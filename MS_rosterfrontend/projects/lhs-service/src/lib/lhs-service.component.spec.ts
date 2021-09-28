import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LhsServiceComponent } from './lhs-service.component';

describe('LhsServiceComponent', () => {
  let component: LhsServiceComponent;
  let fixture: ComponentFixture<LhsServiceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LhsServiceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LhsServiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
