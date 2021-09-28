import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientProfileInfoComponent } from './client-profile-info.component';

describe('ClientProfileInfoComponent', () => {
  let component: ClientProfileInfoComponent;
  let fixture: ComponentFixture<ClientProfileInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientProfileInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientProfileInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
