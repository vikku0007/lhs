import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { EmployeeViewModel } from '../../view-models/employee-view-model';
import { EmpDetailService } from '../../services/emp-detail.service';
import {LoaderService } from 'projects/lhs-service/src/projects';
@Component({
  selector: 'lib-employeedetails',
  templateUrl: './employeedetails.component.html',
  styleUrls: ['./employeedetails.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})
export class EmployeedetailsComponent implements OnInit {

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
  constructor(private route: ActivatedRoute, private empService: EmpDetailService,private loaderService: LoaderService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.employeeid = params.params.id;
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
