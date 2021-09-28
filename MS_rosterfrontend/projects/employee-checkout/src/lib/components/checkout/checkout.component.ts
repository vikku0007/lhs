import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NotificationService, MembershipService } from 'projects/core/src/projects';
import { LogoutService } from '../../services/logout.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'projects/lhs-service/src/projects';
import { ResponseModel } from 'projects/viewmodels/response-model';
import * as moment from 'moment';

@Component({
  selector: 'lib-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  getErrorMessage: 'Please Enter Value';
  rForm: FormGroup;
  shiftId: number = 0;
  employeeId: number = 0;
  responseModel: ResponseModel = {};

  constructor(private fb: FormBuilder, private notificationService: NotificationService, private logoutService: LogoutService,
    private route: ActivatedRoute, private membershipService: MembershipService, private router: Router) {
    // this.route.queryParams.subscribe(params => {
    //   this.shiftId = params['Id'];
    // });
    this.route.paramMap.subscribe((params: any) => {
      this.shiftId = params.params.id;
    });
  }

  ngOnInit(): void {
    this.createForm();
    this.rForm.controls['radioToDo'].setValue('1');
    this.rForm.controls['radioAccident'].setValue('2');
    this.rForm.controls['radioProgress'].setValue('1');
    this.employeeId = this.membershipService.getUserDetails('employeeId');
  }

  createForm() {
    this.rForm = this.fb.group({
      remark: ['', Validators.required],
      radioToDo: [true, Validators.nullValidator],
      radioAccident: [true, Validators.nullValidator],
      radioProgress: [true, Validators.nullValidator],
    });
  }

  logout() {
    if (this.rForm.valid) {
      const data = {
        checkOutRemarks: this.rForm.controls['remark'].value,
        shiftId: Number(this.shiftId),
        employeeId: Number(this.employeeId),
        // checkOutDate: new Date(),
        checkOutDate: moment(),
        checkOutTime: new Date().toTimeString().split(' ')[0],
        totalDuration: 0,
        toDoList_Flag: this.rForm.controls['radioToDo'].value == 2 ? false : true,
        accidentIncident_Flag: this.rForm.controls['radioAccident'].value == 2 ? false : true,
        progressNotes_Flag: this.rForm.controls['radioProgress'].value == 2 ? false : true,
        isCheckoutByWeb: true,
        isCheckoutByApp: false
      }

      this.logoutService.logoutEmployee(data).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: 'Hi, this to confirm you have logged out successfully', title: '' });
            this.rForm.reset();
            this.router.navigate(['employee/dashboard', this.employeeId]);
            break;
          case 0:
            this.notificationService.Warning({ message: this.responseModel.message, title: '' });
            break;
          default:
            break;
        }
      });
    }
  }

}
