import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpCommunicationComponent } from './emp-communication.component';

describe('EmpCommunicationComponent', () => {
  let component: EmpCommunicationComponent;
  let fixture: ComponentFixture<EmpCommunicationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpCommunicationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpCommunicationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
