import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportCoordinatorInfoComponent } from './support-coordinator-info.component';

describe('SupportCoordinatorInfoComponent', () => {
  let component: SupportCoordinatorInfoComponent;
  let fixture: ComponentFixture<SupportCoordinatorInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SupportCoordinatorInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SupportCoordinatorInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
