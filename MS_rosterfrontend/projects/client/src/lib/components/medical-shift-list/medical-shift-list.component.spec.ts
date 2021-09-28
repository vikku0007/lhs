import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MedicalShiftListComponent } from './medical-shift-list.component';

describe('MedicalShiftListComponent', () => {
  let component: MedicalShiftListComponent;
  let fixture: ComponentFixture<MedicalShiftListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MedicalShiftListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MedicalShiftListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
