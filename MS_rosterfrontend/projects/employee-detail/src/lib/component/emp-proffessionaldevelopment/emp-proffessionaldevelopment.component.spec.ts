import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpProffessionaldevelopmentComponent } from './emp-proffessionaldevelopment.component';

describe('EmpProffessionaldevelopmentComponent', () => {
  let component: EmpProffessionaldevelopmentComponent;
  let fixture: ComponentFixture<EmpProffessionaldevelopmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpProffessionaldevelopmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpProffessionaldevelopmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
