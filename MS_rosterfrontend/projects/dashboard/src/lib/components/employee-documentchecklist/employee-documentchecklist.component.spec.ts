import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeDocumentchecklistComponent } from './employee-documentchecklist.component';

describe('EmployeeDocumentchecklistComponent', () => {
  let component: EmployeeDocumentchecklistComponent;
  let fixture: ComponentFixture<EmployeeDocumentchecklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDocumentchecklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDocumentchecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
