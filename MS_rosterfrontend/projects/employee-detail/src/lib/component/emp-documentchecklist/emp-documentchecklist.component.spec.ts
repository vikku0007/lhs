import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpDocumentchecklistComponent } from './emp-documentchecklist.component';

describe('EmpDocumentchecklistComponent', () => {
  let component: EmpDocumentchecklistComponent;
  let fixture: ComponentFixture<EmpDocumentchecklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpDocumentchecklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpDocumentchecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
