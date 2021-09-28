import { Component, OnInit } from '@angular/core';
import {Location} from '@angular/common';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { EmpServiceService } from '../../../services/emp-service.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { EmployeeDetails } from '../../../viewmodel/employee-details';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.scss']
})
export class UserCardComponent implements OnInit {
  employeeId = 0;
  empPrimaryInfo: EmployeeDetails = {};
  response: ResponseModel = {};
  constructor(private route: ActivatedRoute, private _location: Location, private notificationService: NotificationService, private empService: EmpServiceService)
  {}

  backClicked() {
    this._location.back();
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      // tslint:disable-next-line: radix
      this.employeeId = parseInt(params.Id);
    });
    if (this.employeeId > 0) {
     this.GetPrimaryDetails();
    }
  }
  getAge(value)
  {
    let newDate = new Date(value);
    let timeDiff = Math.abs(Date.now() - newDate.getTime());
    let age = Math.floor((timeDiff / (1000 * 3600 * 24))/365.25);
    return age;
  }
  GetPrimaryDetails(){
  
    this.empService.getEmployeePrimaryInfo(this.employeeId).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
          this.empPrimaryInfo = this.response.responseData;
          break;
        default:
          this.notificationService.Success({ message: this.response.message, title: null });
          break;
      }
    });
    }
}

