import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientFundinginfoComponent } from './client-fundinginfo.component';

describe('ClientFundinginfoComponent', () => {
  let component: ClientFundinginfoComponent;
  let fixture: ComponentFixture<ClientFundinginfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientFundinginfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientFundinginfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
