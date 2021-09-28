import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { ClientService } from '../../services/client.service';
import { ClientPrimarycarerInfo } from '../../view-models/client-primary-carerinfo';
import { ClientAdditionalNotes } from '../../view-models/client-additionalnotes';


@Component({
  selector: 'lib-client-additional-notes',
  templateUrl: './client-additional-notes.component.html',
  styleUrls: ['./client-additional-notes.component.css']
})

export class ClientAdditionalNotesComponent implements OnInit, OnChanges {
  @Input() Clientadditionalnotes: ClientAdditionalNotes;
  rForm: FormGroup;
  response: ResponseModel = {};
  @ViewChild('btnadditionalInfoCancel') cancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  getErrorMessage:'Please Enter Value';
  ClientId: any;
  responseModel: ResponseModel = {};
   constructor(private route: ActivatedRoute, private fb: FormBuilder, private clientservice:ClientService, private notificationService: NotificationService) { }

  ngOnChanges(changes: SimpleChanges): void {
     this.route.queryParams.subscribe(params => {
        this.ClientId = parseInt(params['Id']);
      });
    }
  
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.ClientId = parseInt(params['Id']);
    });
  this.createForm();
  this.getClientDetails();
  }

  createForm() {
    this.rForm = this.fb.group({
      PublicNote: [null, Validators.required],
      PrivateNote: [null, Validators.required],
      
    });
  }
  getClientDetails() {
    const data = {
       Id: this.ClientId
     }
     this.clientservice.GetClientDetailPageInfo(data).subscribe(res => {
      this.responseModel = res;
       if (this.responseModel.status > 0) {
         this.Clientadditionalnotes = this.responseModel.responseData.clientAdditionalNote;
        
       }
       else {
         //this.notificationService.Error({ message: this.response.message, title: null });
       }
     });
   }
  editAdditionalDetails() {
    this.rForm.get('PublicNote').patchValue(this.Clientadditionalnotes.publicNote);
    this.rForm.get('PrivateNote').patchValue(this.Clientadditionalnotes.privateNote);
    
  }
  UpdateAdditionalnotes() {
    if (this.rForm.valid) {
      const data = {
        ClientId: this.ClientId,
        PublicNote: this.rForm.value.PublicNote,
        PrivateNote: this.rForm.value.PrivateNote,
        
      }
      this.clientservice.AddClientAdditionalNotes(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.Clientadditionalnotes = this.response.responseData;
            this.cancel.nativeElement.click();
            this.getClientDetails();
            this.notificationService.Success({ message: this.response.message, title: null });
            
            break;

          default:
            this.notificationService.Error({ message: this.response.message, title: null });
            break;
        }
      });
    }
  }

}