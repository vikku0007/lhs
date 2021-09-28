import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { EmpServiceService } from '../../services/emp-service.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { EmployeeViewModel } from '../../viewmodel/employee-view-model';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import {LoaderService } from 'projects/lhs-service/src/projects';
@Component({
  selector: 'lib-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})
export class EmployeeDetailComponent implements OnInit {

  // For Education
  displayedColumnsEducation: string[] = ['institute', 'degree', 'fieldStudy', 'completionDate', 'additionalNotes', 'action'];
  dataSourceEducation: any;
  // For Experience
  displayedColumnsExperience: string[] = ['company', 'jobTitle', 'startDate', 'endDate', 'jobDesc', 'action'];
  dataSourceExperience: any;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  @ViewChild(MatSort, { static: true }) sort2: MatSort;
  responseModel: ResponseModel = {};
  pageData: EmployeeViewModel = {
    employeePrimaryInfo: {},
    employeeKinInfo: { employeeId: 0 },
    employeeAwardInfo: {
      allowances: null,
      systemIdentifier: null,
      dailyhours: null,
      weeklyhours: null
    },
    employeeJobProfile: {},
    employeeDrivingLicenseInfo: {},
    employeeMiscInfo: {},
    employeeEducation: [],
    employeeWorkExp: [],
    employeePayRate: {}
  };
  employeeid = 0;
  constructor(private route: ActivatedRoute, private empService: EmpServiceService,private loaderService: LoaderService) { }

  ngOnInit(): void {
    // this.dataSourceEducation.sort = this.sort;
    // this.dataSourceExperience.sort = this.sort2;

    this.route.queryParams.subscribe(params => {
      // tslint:disable-next-line: radix
      this.employeeid = parseInt(params.Id);
    });

    if (this.employeeid > 0) {
      this.getEmployeeDetails();

    }

  }


  getEmployeeDetails() {
   this.loaderService.start();
    this.empService.getEmployeeDetails(this.employeeid).subscribe(res => {
    this.responseModel = res;
      if (this.responseModel.status > 0) {
       this.loaderService.stop();
        this.pageData = this.responseModel.responseData;
        this.dataSourceEducation = new MatTableDataSource(this.responseModel.responseData.employeeEducation);
        this.dataSourceExperience = new MatTableDataSource(this.responseModel.responseData.employeeWorkExp);
       
      }
      else {
       // alert('Something went wrong');
      }
    });
  }


}
