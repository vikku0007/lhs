import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmenuPageComponent } from './submenu-page.component';

describe('SubmenuPageComponent', () => {
  let component: SubmenuPageComponent;
  let fixture: ComponentFixture<SubmenuPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubmenuPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubmenuPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
