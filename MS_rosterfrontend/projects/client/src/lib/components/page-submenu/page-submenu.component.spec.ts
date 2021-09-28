import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageSubmenuComponent } from './page-submenu.component';

describe('PageSubmenuComponent', () => {
  let component: PageSubmenuComponent;
  let fixture: ComponentFixture<PageSubmenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageSubmenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageSubmenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
