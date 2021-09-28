import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentCategoryComponent } from './incident-category.component';

describe('IncidentCategoryComponent', () => {
  let component: IncidentCategoryComponent;
  let fixture: ComponentFixture<IncidentCategoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentCategoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
