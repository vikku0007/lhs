import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientGuardianiInformationComponent } from './client-guardiani-information.component';

describe('ClientGuardianiInformationComponent', () => {
  let component: ClientGuardianiInformationComponent;
  let fixture: ComponentFixture<ClientGuardianiInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientGuardianiInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientGuardianiInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
