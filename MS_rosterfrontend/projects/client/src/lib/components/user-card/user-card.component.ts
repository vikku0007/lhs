import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { ApiService, MembershipService, NotificationService } from 'projects/core/src/projects';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ClientService } from '../../services/client.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { AddClientDetails } from '../../view-models/add-client-details';
import { ActivatedRoute } from '@angular/router';
import { ClientMedicalHistory } from '../../view-models/client-medical-history';
import { ClientBasicData } from '../../view-models/client-basicinfo';

@Component({
  selector: 'lib-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.scss']
})
export class UserCardComponent implements OnInit {
  ClientId: number;
  @Input() BasicInfo: ClientBasicData;
  responseModel: ResponseModel = {};
  constructor(private clientService: ClientService, private membershipService: MembershipService,
    private notificationService: NotificationService, private fb: FormBuilder, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.ClientId = parseInt(params['Id']);
    });
    this.getClientDetails();
  }
  getClientDetails() {
    const data = {
      Id: this.ClientId
    }
    this.clientService.GetClientDetailPageInfo(data).subscribe(res => {
      this.responseModel = res;
      if (this.responseModel.status > 0) {
        this.BasicInfo = this.responseModel.responseData.clientPrimaryInfo;
        this.BasicInfo.fullName = this.BasicInfo.firstName + ' ' + (this.BasicInfo?.middleName ? this.BasicInfo.middleName : ' ') + ' ' +
          this.BasicInfo.lastName;
      }
      else {
        this.notificationService.Error({ message: this.responseModel.message, title: null });
      }
    });
  }
}
