import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterEntriesComponent } from './master-entries.component';

describe('MasterEntriesComponent', () => {
  let component: MasterEntriesComponent;
  let fixture: ComponentFixture<MasterEntriesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterEntriesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterEntriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
