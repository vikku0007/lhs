import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ClientPrimaryCareerInfoComponent } from './client-primarycareerinfo.component';

describe('ClientPrimaryCareerInfoComponent', () => {
  let component: ClientPrimaryCareerInfoComponent;
  let fixture: ComponentFixture<ClientPrimaryCareerInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientPrimaryCareerInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientPrimaryCareerInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
