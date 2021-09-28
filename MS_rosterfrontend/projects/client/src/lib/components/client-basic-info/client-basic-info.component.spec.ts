import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientBasicInfoComponent } from './client-basic-info.component';

describe('ClientBasicInfoComponent', () => {
  let component: ClientBasicInfoComponent;
  let fixture: ComponentFixture<ClientBasicInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientBasicInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientBasicInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
