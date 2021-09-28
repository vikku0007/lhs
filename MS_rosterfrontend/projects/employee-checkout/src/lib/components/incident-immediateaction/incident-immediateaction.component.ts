import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NotificationService, MembershipService } from 'projects/core/src/projects';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import * as moment from 'moment';
import { APP_DATE_FORMATS, AppDateAdapter } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { environment } from 'src/environments/environment';
import { LogoutService } from '../../services/logout.service';
import { IncidentImpactedPerson } from '../../view-models/accident-incident';
@Component({
  selector: 'lib-incident-immediateaction',
  templateUrl: './incident-immediateaction.component.html',
  styleUrls: ['./incident-immediateaction.component.scss'],
  providers: [
    {
      provide: DateAdapter, useClass: AppDateAdapter
    },
    {
      provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
  ]
})

export class IncidentImmediateactionComponent implements OnInit {
  getErrorMessage: 'Please Enter Value';
  response: ResponseModel = {};
  rFormAction: FormGroup;
  clientId = 0;
  todayDatemax = new Date();
  @ViewChild('btnEditaccidentCancel') editCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('fileInput', { static: false }) fileInput: ElementRef;
  textImmediate: string;
  shiftId: number;
  employeeId: any;
  constructor(private fb: FormBuilder, private notification: NotificationService,
    private route: ActivatedRoute, private logoutservice: LogoutService, private membershipService: MembershipService,
    private commonservice: CommonService) {
    this.route.paramMap.subscribe((params: any) => {
      this.clientId = Number(params.params.id);
      this.shiftId = Number(params.params.shiftId);
    });
    this.employeeId = this.membershipService.getUserDetails('employeeId');
  }

  ngOnInit(): void {
    this.createFormAction();
    this.getIncidentImmediateAction();
    this.Setvalue();
  }
  Setvalue() {
    this.rFormAction.controls['IsFamilyAware'].setValue("2");
    this.rFormAction.controls['IsUnder18'].setValue("2");
    this.rFormAction.controls['IsPoliceInformed'].setValue("2");
  }
  createFormAction() {
    this.rFormAction = this.fb.group({
      IsPoliceInformed: ['', Validators.nullValidator],
      OfficerName: ['', Validators.nullValidator],
      PoliceStation: ['', Validators.nullValidator],
      PoliceNumber: ['', Validators.nullValidator],
      PoliceNotInform: [null, Validators.nullValidator],
      IsFamilyAware: ['', Validators.nullValidator],
      Guardian: ['', Validators.required],
      IsUnder18: ['', Validators.nullValidator],
      ChildContacted: ['', Validators.required],
      DescribeImmediate: ['', Validators.required],
      WorkerDescribe: ['', Validators.required],
      DescribeDisability: ['', Validators.required],
    });
  }

  radioImpactedChange(val) {
    if (val.value == '1' || val.value == '3') {
      this.rFormAction.controls['Guardian'].setValidators(null);
      this.rFormAction.controls['Guardian'].updateValueAndValidity();
    } else {
      this.rFormAction.controls['Guardian'].setValidators(Validators.required);
      this.rFormAction.controls['Guardian'].updateValueAndValidity();
    }
  }

  radio18Change(val) {
    if (val.value == '1' || val.value == '3' || val.value == '4') {
      this.rFormAction.controls['ChildContacted'].setValidators(null);
      this.rFormAction.controls['ChildContacted'].updateValueAndValidity();
    } else {
      this.rFormAction.controls['ChildContacted'].setValidators(Validators.required);
      this.rFormAction.controls['ChildContacted'].updateValueAndValidity();
    }
  }

  getIncidentImmediateAction() {
    const data = {
      Id: this.clientId,
      ShiftId: this.shiftId

    };
    this.logoutservice.getIncidentImmediateAction(data).subscribe(res => {
      this.response = res;
      if (this.response.status > 0) {
        if (this.response.responseData.incidentImmediateAction.clientId > 0) {
          this.rFormAction.controls['IsPoliceInformed'].patchValue(this.response.responseData.incidentImmediateAction.isPoliceInformed == true ? "1" : "2");
          this.rFormAction.controls['OfficerName'].patchValue(this.response.responseData.incidentImmediateAction.officerName);
          this.rFormAction.controls['PoliceStation'].patchValue(this.response.responseData.incidentImmediateAction.policeStation);
          this.rFormAction.controls['PoliceNumber'].patchValue(this.response.responseData.incidentImmediateAction.policeNo);
          this.rFormAction.controls['PoliceNotInform'].patchValue(this.response.responseData.incidentImmediateAction.providerPosition);
          this.rFormAction.controls['IsFamilyAware'].patchValue(this.response.responseData.incidentImmediateAction.isFamilyAware == 1 ? "1" : this.response.responseData.incidentImmediateAction.isFamilyAware == 2 ? "2" : this.response.responseData.incidentImmediateAction.isFamilyAware == 3 ? "3" : "")
          this.rFormAction.controls['IsUnder18'].patchValue(this.response.responseData.incidentImmediateAction.isUnder18 == 1 ? "1" : this.response.responseData.incidentImmediateAction.isUnder18 == 2 ? "2" : this.response.responseData.incidentImmediateAction.isUnder18 == 3 ? "3" : this.response.responseData.incidentImmediateAction.isUnder18 == 4 ? "4" : "")
          this.rFormAction.controls['Guardian'].patchValue(this.response.responseData.incidentImmediateAction.contacttoFamily);
          this.rFormAction.controls['ChildContacted'].patchValue(this.response.responseData.incidentImmediateAction.contactChildProtection);
          this.rFormAction.controls['DescribeImmediate'].patchValue(this.response.responseData.incidentImmediateAction.disabilityPerson);
          this.rFormAction.controls['WorkerDescribe'].patchValue(this.response.responseData.incidentImmediateAction.subjectWorkerAllegation);
          this.rFormAction.controls['DescribeDisability'].patchValue(this.response.responseData.incidentImmediateAction.subjectDisabilityPerson);
          this.textImmediate = "Update";
        }
        else {
          this.textImmediate = "Submit"
        }

      }
    });
  }
  AddActionTaken() {
    if (this.rFormAction.valid) {
      const data = {
        'ClientId': this.clientId,
        'ShiftId': this.shiftId,
        'EmployeeId': this.employeeId,
        'IsPoliceInformed': this.rFormAction.value.IsPoliceInformed == "1" ? true : false,
        'OfficerName': this.rFormAction.value.OfficerName,
        'PoliceStation': this.rFormAction.value.PoliceStation,
        'PoliceNo': this.rFormAction.value.PoliceNumber,
        'ProviderPosition': this.rFormAction.value.PoliceNotInform,
        'ContacttoFamily': this.rFormAction.value.Guardian,
        'IsFamilyAware': this.rFormAction.value.IsFamilyAware == "1" ? this.rFormAction.value.IsFamilyAware = 1 : this.rFormAction.value.IsFamilyAware == "2" ? this.rFormAction.value.IsFamilyAware = 2 : this.rFormAction.value.IsFamilyAware == "3" ? this.rFormAction.value.IsFamilyAware = 3 : this.rFormAction.value.IsFamilyAware = 0,
        'IsUnder18': this.rFormAction.value.IsUnder18 == "1" ? this.rFormAction.value.IsUnder18 = 1 : this.rFormAction.value.IsUnder18 == "2" ? this.rFormAction.value.IsUnder18 = 2 : this.rFormAction.value.IsUnder18 == "3" ? this.rFormAction.value.IsUnder18 = 3 : this.rFormAction.value.IsUnder18 == "4" ? this.rFormAction.value.IsUnder18 = 4 : this.rFormAction.value.IsUnder18 = 0,
        'ContactChildProtection': this.rFormAction.value.ChildContacted,
        'DisabilityPerson': this.rFormAction.value.DescribeImmediate,
        'SubjectWorkerAllegation': this.rFormAction.value.WorkerDescribe,
        'SubjectDisabilityPerson': this.rFormAction.value.DescribeDisability,
      };
      this.logoutservice.AddIncidentImmediateAction(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.notification.Success({ message: this.response.message, title: null });
            this.getIncidentImmediateAction()
            break;
          default:
            this.notification.Warning({ message: this.response.message, title: null });
            break;
        }
      });
    }
  }


}
