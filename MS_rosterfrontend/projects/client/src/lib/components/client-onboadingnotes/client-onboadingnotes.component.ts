import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { ClientService } from '../../services/client.service';
import { ClientPrimarycarerInfo } from '../../view-models/client-primary-carerinfo';
import { ClientOnBoardinNotes } from '../../view-models/client-onboardingnotes';

@Component({
  selector: 'lib-client-onboadingnotes',
  templateUrl: './client-onboadingnotes.component.html',
  styleUrls: ['./client-onboadingnotes.component.scss']
})

export class ClientOnboadingnotesComponent implements OnInit, OnChanges {
  @Input() Clientonboardingnotes: ClientOnBoardinNotes;
  rForm: FormGroup;
  response: ResponseModel = {};
  @ViewChild('btnonboadingCancel') cancel: ElementRef;
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
      CareNotes: [null, Validators.required],
       
    });
  }
  getClientDetails() {
   const data = {
      Id: this.ClientId
    }
    this.clientservice.GetClientDetailPageInfo(data).subscribe(res => {
     this.responseModel = res;
      if (this.responseModel.status > 0) {
        this.Clientonboardingnotes = this.responseModel.responseData.clientBoadingNotes;
       
      }
      else {
        //this.notificationService.Error({ message: this.response.message, title: null });
      }
    });
  }
  editonboardingnotesDetails() {
    this.rForm.get('CareNotes').patchValue(this.Clientonboardingnotes.careNote);
    // this.rForm.get('CareNotesClientProvided').patchValue(this.Clientonboardingnotes.careNoteByClient);
    
  }
  UpdateOnBoardingnotes() {
    if (this.rForm.valid) {
      const data = {
        ClientId: this.ClientId,
        CareNote: this.rForm.value.CareNotes,
        
        
      }
      this.clientservice.AddClientOnBoardingNotes(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.Clientonboardingnotes = this.response.responseData;
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