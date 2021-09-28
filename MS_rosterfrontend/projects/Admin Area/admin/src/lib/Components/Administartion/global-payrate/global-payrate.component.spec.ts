import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GlobalPayrateComponent } from './global-payrate.component';

describe('GlobalPayrateComponent', () => {
  let component: GlobalPayrateComponent;
  let fixture: ComponentFixture<GlobalPayrateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GlobalPayrateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GlobalPayrateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
