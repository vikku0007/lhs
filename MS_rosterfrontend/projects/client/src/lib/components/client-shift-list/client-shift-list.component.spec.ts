import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientShiftListComponent } from './client-shift-list.component';

describe('ClientShiftListComponent', () => {
  let component: ClientShiftListComponent;
  let fixture: ComponentFixture<ClientShiftListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientShiftListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientShiftListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
