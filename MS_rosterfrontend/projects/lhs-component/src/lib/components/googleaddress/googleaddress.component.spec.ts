import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GoogleaddressComponent } from './googleaddress.component';

describe('GoogleaddressComponent', () => {
  let component: GoogleaddressComponent;
  let fixture: ComponentFixture<GoogleaddressComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GoogleaddressComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GoogleaddressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
