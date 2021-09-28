import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientFundingListComponent } from './client-funding-list.component';

describe('ClientFundingListComponent', () => {
  let component: ClientFundingListComponent;
  let fixture: ComponentFixture<ClientFundingListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientFundingListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientFundingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
