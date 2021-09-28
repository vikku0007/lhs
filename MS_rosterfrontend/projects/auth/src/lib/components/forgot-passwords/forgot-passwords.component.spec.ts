import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgotPasswordsComponent } from './forgot-passwords.component';

describe('ForgotPasswordsComponent', () => {
  let component: ForgotPasswordsComponent;
  let fixture: ComponentFixture<ForgotPasswordsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ForgotPasswordsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ForgotPasswordsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
