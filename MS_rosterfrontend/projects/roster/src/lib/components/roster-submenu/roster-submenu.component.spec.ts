import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RosterSubmenuComponent } from './roster-submenu.component';

describe('RosterSubmenuComponent', () => {
  let component: RosterSubmenuComponent;
  let fixture: ComponentFixture<RosterSubmenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RosterSubmenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RosterSubmenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
