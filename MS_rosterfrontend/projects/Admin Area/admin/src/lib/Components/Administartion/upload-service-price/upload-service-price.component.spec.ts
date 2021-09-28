import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadServicePriceComponent } from './upload-service-price.component';

describe('UploadServicePriceComponent', () => {
  let component: UploadServicePriceComponent;
  let fixture: ComponentFixture<UploadServicePriceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UploadServicePriceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadServicePriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
