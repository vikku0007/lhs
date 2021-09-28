import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientOnboadingnotesComponent } from './client-onboadingnotes.component';

describe('ClientOnboadingnotesComponent', () => {
  let component: ClientOnboadingnotesComponent;
  let fixture: ComponentFixture<ClientOnboadingnotesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientOnboadingnotesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientOnboadingnotesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
