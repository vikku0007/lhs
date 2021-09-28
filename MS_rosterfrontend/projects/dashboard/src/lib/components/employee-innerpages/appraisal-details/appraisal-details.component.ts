import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { EmployeeDetails } from '../../../viewmodel/employee-details';
import { EmployeeJobProfile } from '../../../viewmodel/employee-jobprofile';
import { ActivatedRoute } from '@angular/router';
import { EmpServiceService } from '../../../services/emp-service.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { FormGroup, FormBuilder, Validators, FormControl, FormArray, ReactiveFormsModule, NgForm } from '@angular/forms';
import { EmployeeAppraisalStandards } from '../view-model/employee-appraisal-standards';
import { NotificationService } from 'projects/core/src/projects';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { Paging } from 'projects/viewmodels/paging';
import { stringify } from 'querystring';
import { isBuffer } from 'util';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import * as moment from 'moment';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators'
// For Appraisal Type
interface AppraisalType {
  value: string;
  name: string;
}

// For Discussion
export interface PeriodicElementStandard {
  description: string;
  IsAchieves: boolean;
  IsExceeds: boolean;
  IsBelow: boolean;
}

// For Education
const ELEMENT_DATA_STANDARD: PeriodicElementStandard[] = [
  { description: 'Job Knowledge', IsAchieves: true, IsExceeds: true, IsBelow: true },
  { description: 'Quality Of Work', IsAchieves: false, IsExceeds: false, IsBelow: false },
  { description: 'Productivity', IsAchieves: false, IsExceeds: false, IsBelow: false },
  { description: 'Dependability', IsAchieves: false, IsExceeds: false, IsBelow: false },
  { description: 'Attendance', IsAchieves: false, IsExceeds: false, IsBelow: false },
  { description: 'Relations With Others', IsAchieves: false, IsExceeds: false, IsBelow: false },
  { description: 'Commitment To Safety', IsAchieves: false, IsExceeds: false, IsBelow: false },
  { description: 'Supervisory Ability', IsAchieves: false, IsExceeds: false, IsBelow: false },
  { description: 'Overall Appraisal Rating', IsAchieves: false, IsExceeds: false, IsBelow: false },
];

@Component({
  selector: 'employee-appraisal-details',
  templateUrl: './appraisal-details.component.html',
  styleUrls: ['./appraisal-details.component.scss']
})
export class AppraisalDetailsComponent implements OnInit {

  getErrorMessage: 'Please Enter Value';

  // For Person
  atypes: AppraisalType[] = [
    { value: '', name: 'Select Appraisal Type' },
    { value: '1', name: 'Annualy' },
    { value: '2', name: 'Half Yearly' }
  ];
  selectedAppraisal = 'atype-1';

  // For Discussion
 
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  empPersonalDetails: EmployeeDetails = {};
  empJobDetails: EmployeeJobProfile = {};
  employeeId = 0;
  responseModel: ResponseModel = {};
  rForm: FormGroup;
  rForm1: FormGroup;
  excessArray: { description?: string }[] = [];
  acheivesArray: { description?: string }[] = [];
  belowArray: { description?: string }[] = [];
  appraisalStandards: EmployeeAppraisalStandards = {};
  appraisalStandardsArray: EmployeeAppraisalStandards[] = [];
  
  AppraisalListdata = [];
  Appraisalcrieteriadata = [];
  listArray: [];
  employeePrimaryInfo: {};
  paging: Paging = {};
  AppraisalList = [];
  AppraisalcrieteriaList = [];
  EditAppraisalList = [];
  deleteAppraisalId: number;
  response: ResponseModel = {};
  AppraisalInfoModel: AllEmployeeAppraisalInfo = {};
  ArrayList = [];
  AppraisalTypeList: any;
  employeeexcessList: EmployeeAppraisalStandards[];
  employeachievesList: EmployeeAppraisalStandards[];
  employeebelowList: EmployeeAppraisalStandards[];
  EditList: AllEmployeeAppraisalInfo[] = [];
  displayedColumnsStandard: string[] = ['description', 'exceedsStandards', 'achievesStandards', 'belowStandards'];
  dataSourceStandard = new MatTableDataSource(this.AppraisalcrieteriaList);
  displayedColumns: string[] = ['sr', 'appraisalTypeName', 'appraisalDateFrom', 'appraisalDateTo', 'createdDate', 'action'];
  dataSource = new MatTableDataSource(this.AppraisalList);
  totalCount: any;
  EditAppraisalId: any;
  textName: string;
  orderBy: number;
  orderColumn: number;
  todayDate: Date = new Date();
  EditAppraisal = new FormGroup({
    IsAchieves: new FormControl(''),
    IsExceeds: new FormControl(''),
    IsBelow: new FormControl(''),
  });
  EditId: number;
  startDate: any;
  detailList: any;
  WarningData: any;
  datetoday: any;
  totalwarning: any;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  // tslint:disable-next-line: max-line-length
  constructor(private route: ActivatedRoute, private empService: EmpServiceService, private fb: FormBuilder, private notificationService: NotificationService,
    private commonservice: CommonService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.textName = 'Submit';
    this.route.queryParams.subscribe(params => {      
      this.employeeId = parseInt(params.Id);
      this.EditId = parseInt(params.EId);
      if (this.EditId > 0) {
        this.getappraisalById();
        this.getAppraisalListById();

      }
     // this.rForm.controls['toDate'].disable();

    });
    // tslint:disable-next-line: no-unused-expression
    // this.employeeId > 0 ? this.getEmployeeDetails() : null;
    if (this.textName == 'Submit') {
      this.createForm();
  }

    this.createFormEdit();
    this.GetAppraisalType();
    this.getEmployeeAppraisalList();
    this.getAppraisalCrieteria();
    this.getTotalWarning();

  }
  ngAfterViewInit(): void {
  setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getEmployeeAppraisalList())
        )
        .subscribe();
    }, 2000);

  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
     switch (sortColumn) {
       case 'appraisalType':
          this.orderColumn = 1;
          break;
          case 'fromDate':
          this.orderColumn = 2;
          break;
          case 'toDate':
          this.orderColumn = 3;
          break;
         case 'createdDate':
         this.orderColumn = 4;
         break;

      default:
        break;
    }
  }
  removeSpace(value) {

    let a = value.replace(/\s/g, '');
    return a.toLowerCase();
  }
  OnSubmit() {
    if (this.textName == 'Submit') {
      this.AddEmployeeDetails();
    }
    else if (this.textName == 'Update') {
      this.EditEmployeeAppraisalDetails();
    }
  }

  getappraisalById() {
    const data = {
      Id: Number(this.EditId),
    };
    this.empService.GetEmployeeAppraisaldetails(data).subscribe(res => {
      this.response = res;
      this.EditList = this.response.responseData;
      this.EditAppraisalId = this.EditList[0]['id'];
      this.rForm.controls.appraisalType.setValue(this.EditList[0]['appraisalType']);
      this.rForm.controls.fromDate.setValue(this.EditList[0]['appraisalDateFrom']);
      this.rForm.controls.toDate.setValue(this.EditList[0]['appraisalDateTo']);
      //this.rForm.get('toDate').disable();
      this.textName = 'Update';

    })
  }

  createForm() {
    this.rForm = this.fb.group({
      appraisalType: ['', Validators.required],
      fromDate: ['', Validators.required],
      toDate: ['', Validators.required],
      exceeds: ['', null],
      achieves: ['', null],
      below: ['', null],
    });
  }
  createFormEdit() {
    this.rForm1 = this.fb.group({
      appraisalType: ['', Validators.required],
      fromDate: ['', Validators.required],
      toDate: ['', Validators.required],
      exceeds: ['', null],
      achieves: ['', null],
      below: ['', null],
    });
  }


  // calculateDate() {    

  //   if (this.rForm.value.appraisalType == "61") {
  //     this.todayDate = this.rForm.value.fromDate;
  //     var newDate = moment(this.todayDate).add(1, 'year').format('YYYY-MM-DD').toString();
  //     var subratctday=moment(newDate).subtract(1,'day').format('YYYY-MM-DD').toString();
  //     this.rForm.controls.toDate.setValue(subratctday);
  //     this.rForm.get('toDate').disable();
  //   }
  //   else if (this.rForm.value.appraisalType == "62") {
  //     this.todayDate = this.rForm.value.fromDate;
  //     var newDate = moment(this.todayDate).add(6, 'month').format('YYYY-MM-DD').toString();
  //     var subratctday=moment(newDate).subtract(1,'day').format('YYYY-MM-DD').toString();
  //     this.rForm.controls.toDate.setValue(subratctday);
  //     this.rForm.get('toDate').disable();

  //   }
  // }
  getExcess(data: any, isChecked: any) {    
   
    if (isChecked.currentTarget.checked) {
      (document.getElementById(this.removeSpace(data.description) + 'achieves') as HTMLInputElement).disabled = true;
      (document.getElementById(this.removeSpace(data.description) + 'below') as HTMLInputElement).disabled = true;
      (document.getElementById(this.removeSpace(data.description) + 'achieves') as HTMLInputElement).checked = false;
      (document.getElementById(this.removeSpace(data.description) + 'below') as HTMLInputElement).checked = false;
      const index = this.EditAppraisalList.findIndex(x => x.descriptionName === data.description);
      if (index > -1) {
        this.EditAppraisalList[index].isAcheives = false;
        this.EditAppraisalList[index].isBelow = false;
      }
     this.removeData(data.description);
      this.excessArray.push({description:data.description});
    } else {
      const index = this.excessArray.findIndex(x => x.description === data.description);
      this.excessArray.splice(index, 1);
      (document.getElementById(this.removeSpace(data.description) + 'achieves') as HTMLInputElement).disabled = false;
      (document.getElementById(this.removeSpace(data.description) + 'below') as HTMLInputElement).disabled = false;

    }
    console.log(this.excessArray);
  }

  removeData(description : string){
    debugger;
    const indexacheivesArray = this.acheivesArray.findIndex(x => x.description === description);
    if(indexacheivesArray > -1){
    this.acheivesArray.splice(indexacheivesArray, 1);
    }
    const indexbelowArray = this.belowArray.findIndex(x => x.description === description);
    if(indexbelowArray > -1){
    this.belowArray.splice(indexbelowArray, 1);
    }
    const indexexcessArray = this.excessArray.findIndex(x => x.description === description);
    if(indexexcessArray > -1){
    this.excessArray.splice(indexexcessArray, 1);
    }
  }

  getBelow(data: any, isChecked: any) {   
    debugger; 
    if (isChecked.currentTarget.checked) {
      (document.getElementById(this.removeSpace(data.description) + 'achieves') as HTMLInputElement).disabled = true;
      (document.getElementById(this.removeSpace(data.description) + 'exceeds') as HTMLInputElement).disabled = true;
      (document.getElementById(this.removeSpace(data.description) + 'achieves') as HTMLInputElement).checked = false;
      (document.getElementById(this.removeSpace(data.description) + 'exceeds') as HTMLInputElement).checked = false;
      const index = this.EditAppraisalList.findIndex(x => x.descriptionName === data.description);
      if (index > -1) {
        this.EditAppraisalList[index].isAcheives = false;
        this.EditAppraisalList[index].isExceeds = false;
      }
      this.removeData(data.description);
      this.belowArray.push({description:data.description});
    } else {
      const index = this.belowArray.findIndex(x => x.description === data.description);
      this.belowArray.splice(index, 1);
      (document.getElementById(this.removeSpace(data.description) + 'achieves') as HTMLInputElement).disabled = false;
      (document.getElementById(this.removeSpace(data.description) + 'exceeds') as HTMLInputElement).disabled = false;
    }
    console.log(this.belowArray);
  }

  getAcheives(data: any, isChecked: any) {
    debugger; 
    if (isChecked.currentTarget.checked) {
      (document.getElementById(this.removeSpace(data.description) + 'exceeds') as HTMLInputElement).disabled = true;
      (document.getElementById(this.removeSpace(data.description) + 'below') as HTMLInputElement).disabled = true;
      (document.getElementById(this.removeSpace(data.description) + 'exceeds') as HTMLInputElement).checked = false;
      (document.getElementById(this.removeSpace(data.description) + 'below') as HTMLInputElement).checked = false;
      const index = this.EditAppraisalList.findIndex(x => x.descriptionName === data.description);
      if (index > -1) {
        this.EditAppraisalList[index].isBelow = false;
        this.EditAppraisalList[index].isExceeds = false;
      }
      
      this.removeData(data.description);
      this.acheivesArray.push({description:data.description});
    } else {
      const index = this.acheivesArray.findIndex(x => x.description === data.description);
      this.acheivesArray.splice(index, 1);
      (document.getElementById(this.removeSpace(data.description) + 'exceeds') as HTMLInputElement).disabled = false;
      (document.getElementById(this.removeSpace(data.description) + 'below') as HTMLInputElement).disabled = false;
    }
    console.log(this.acheivesArray);
  }

  // getEmployeeDetails() {
  //   this.empService.getEmployeeDetails(this.employeeId).subscribe(res => {
  //     this.responseModel = res;
  //     switch (this.responseModel.status) {
  //       case 1:
  //         this.rForm.controls.name.patchValue(this.responseModel.responseData.employeePrimaryInfo.firstName + ' ' +
  //           this.responseModel.responseData.employeePrimaryInfo.lastName);
  //         this.rForm.controls.employeeId.patchValue(this.responseModel.responseData.employeePrimaryInfo.employeeId);
  //         this.rForm.controls.departmentName.patchValue(this.responseModel.responseData.employeeJobProfile.departmentName);
  //         break;

  //       default:
  //         break;
  //     }

  //   });
  // }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getEmployeeAppraisalList();
  }
  getEmployeeAppraisalList() {
     this.getSortingOrder();
    const data = {
      EmployeeId: Number(this.employeeId),
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.empService.getEmployeeAppraisalList(data).subscribe((res: any) => {
      this.totalCount = res.total;
      this.response = res;
      switch (this.response.status) {
        case 1:
          this.AppraisalList = this.response.responseData;
          this.dataSource = new MatTableDataSource(this.AppraisalList);
          break;
          default:
            this.dataSource = new MatTableDataSource(this.AppraisalListdata);
            break;
      }
    });
  }
  openEditDetails(elem) {    
    this.textName = 'Update';
    this.EditAppraisalId = elem.id;
    this.rForm.controls.appraisalType.setValue(elem.appraisalType);
    this.rForm.controls.fromDate.setValue(elem.appraisalDateFrom);
    this.rForm.controls.toDate.setValue(elem.appraisalDateTo);
   // this.rForm.get('toDate').disable();
    this.AppraisalListdata = this.dataSourceStandard.filteredData;
    const data = {
      Id: this.EditAppraisalId
    };
    this.employeeexcessList = [];
    this.empService.getEmployeeAppraisalList(data).subscribe((res: any) => {
      if (res) {
        if (res.responseData != null) {
          this.EditAppraisalList = res.responseData;
        }
      }      
      debugger;
      const Studentarray = [];
      const achievesarray = [];
      const belowarray = [];
      this.EditAppraisalList.forEach(element => {
        const studentdata = {
          DescriptionName: element.descriptionName,
          IsExceeds: element.isExceeds,
        };
        Studentarray.push(studentdata);
        this.employeeexcessList = Studentarray;
      });
      this.EditAppraisalList.forEach(element => {
        const achievesdata = {
          DescriptionName: element.descriptionName,
          IsAchieves: element.isAchieves,
        };
        achievesarray.push(achievesdata);
        this.employeachievesList = achievesarray;
      });
      this.EditAppraisalList.forEach(element => {
        const belowdata = {
          DescriptionName: element.descriptionName,
          IsBelow: element.isBelow,
        };
        belowarray.push(belowdata);
        this.employeebelowList = belowarray;
      });
      this.acheivesArray = [];
      this.excessArray = [];
      this.belowArray = [];
      for (let i = 0; i < this.AppraisalListdata.length; i++) {
        console.log('nanan', this.removeSpace(this.AppraisalListdata[i].description) + 'achieves');
        // this.appraisalStandards = {};
        if (this.employeachievesList.findIndex(x => x.DescriptionName === this.AppraisalListdata[i].description && x.IsAchieves === true) > -1) {
          (document.getElementById(this.removeSpace(this.AppraisalListdata[i].description) + 'achieves') as HTMLInputElement).checked = true;
          this.acheivesArray.push({description:this.AppraisalListdata[i].description});
       
        } else {
          (document.getElementById(this.removeSpace(this.AppraisalListdata[i].description) + 'achieves') as HTMLInputElement).checked = false;
        }
        if (this.employeebelowList.findIndex(x => x.DescriptionName === this.AppraisalListdata[i].description && x.IsBelow === true) > -1) {
          (document.getElementById(this.removeSpace(this.AppraisalListdata[i].description) + 'below') as HTMLInputElement).checked = true;
          this.belowArray.push({description:this.AppraisalListdata[i].description});
        } else {
          (document.getElementById(this.removeSpace(this.AppraisalListdata[i].description) + 'below') as HTMLInputElement).checked = false;
        }
        if (this.employeeexcessList.findIndex(x => x.DescriptionName === this.AppraisalListdata[i].description && x.IsExceeds === true) > -1) {
          (document.getElementById(this.removeSpace(this.AppraisalListdata[i].description) + 'exceeds') as HTMLInputElement).checked = true;
          this.excessArray.push({description:this.AppraisalListdata[i].description});
        } else {
          (document.getElementById(this.removeSpace(this.AppraisalListdata[i].description) + 'exceeds') as HTMLInputElement).checked = false;
        }
      }
      console.log("acheivesArray",this.acheivesArray);
      console.log("excessArray",this.excessArray);
      console.log("belowArray",this.belowArray);

    });
  }
  getAppraisalListById() {
    this.AppraisalListdata = this.dataSourceStandard.filteredData;
    const data = {
      Id: this.EditId
    };
    this.employeeexcessList = [];
    this.empService.getEmployeeAppraisalList(data).subscribe((res: any) => {      
      if (res) {
        if (res.responseData != null) {
          this.EditAppraisalList = res.responseData;
        }
      }
      const Studentarray = [];
      const achievesarray = [];
      const belowarray = [];
      this.EditAppraisalList.forEach(element => {
        const studentdata = {
          DescriptionName: element.descriptionName,
          IsExceeds: element.isExceeds,
        };
        Studentarray.push(studentdata);
        this.employeeexcessList = Studentarray;
      });
      this.EditAppraisalList.forEach(element => {
        const achievesdata = {
          DescriptionName: element.descriptionName,
          IsAchieves: element.isAchieves,
        };
        achievesarray.push(achievesdata);
        this.employeachievesList = achievesarray;
      });
      this.EditAppraisalList.forEach(element => {
        const belowdata = {
          DescriptionName: element.descriptionName,
          IsBelow: element.isBelow,
        };
        belowarray.push(belowdata);
        this.employeebelowList = belowarray;
      });
      for (let i = 0; i < this.AppraisalListdata.length - 1; i++) {
        console.log('nanan', this.removeSpace(this.AppraisalListdata[i].description) + 'achieves');
        // this.appraisalStandards = {};
        if (this.employeachievesList.findIndex(x => x.DescriptionName === this.AppraisalListdata[i].description && x.IsAchieves === true) > -1) {
          (document.getElementById(this.removeSpace(this.AppraisalListdata[i].description) + 'achieves') as HTMLInputElement).checked = true;
        } else {
          (document.getElementById(this.removeSpace(this.AppraisalListdata[i].description) + 'achieves') as HTMLInputElement).checked = false;
        }
        if (this.employeebelowList.findIndex(x => x.DescriptionName === this.AppraisalListdata[i].description && x.IsBelow === true) > -1) {
          (document.getElementById(this.removeSpace(this.AppraisalListdata[i].description) + 'below') as HTMLInputElement).checked = true;
        } else {
          (document.getElementById(this.removeSpace(this.AppraisalListdata[i].description) + 'below') as HTMLInputElement).checked = false;
        }
        if (this.employeeexcessList.findIndex(x => x.DescriptionName === this.AppraisalListdata[i].description && x.IsExceeds === true) > -1) {
          (document.getElementById(this.removeSpace(this.AppraisalListdata[i].description) + 'exceeds') as HTMLInputElement).checked = true;
        } else {
          (document.getElementById(this.removeSpace(this.AppraisalListdata[i].description) + 'exceeds') as HTMLInputElement).checked = false;
        }
      }
    });
  }
  AddEmployeeDetails() {
    if (this.rForm.valid) {
      // Acheives Array
      for (let i = 0; i < this.acheivesArray.length; i++) {
        this.appraisalStandards = {};
        const index = this.appraisalStandardsArray.findIndex(x => x.DescriptionName === this.acheivesArray[i].description);
        if (index > -1) {
          this.appraisalStandardsArray[index].IsAchieves = true;
        } else {
          this.appraisalStandards.DescriptionName = this.acheivesArray[i].description;
          this.appraisalStandards.IsAchieves = true;
          this.appraisalStandards.IsBelow = false;
          this.appraisalStandards.IsExceeds = false;
          this.appraisalStandards.EmployeeId = this.employeeId;
          this.appraisalStandardsArray.push(this.appraisalStandards);
        }
      }
      // Acheives Array
      for (let i = 0; i < this.belowArray.length; i++) {
        this.appraisalStandards = {};
        const index = this.appraisalStandardsArray.findIndex(x => x.DescriptionName === this.belowArray[i].description);
        if (index > -1) {
          this.appraisalStandardsArray[index].IsBelow = true;
        } else {
          this.appraisalStandards.DescriptionName = this.belowArray[i].description;
          this.appraisalStandards.IsBelow = true;
          this.appraisalStandards.IsExceeds = false;
          this.appraisalStandards.IsAchieves = false;
          this.appraisalStandards.EmployeeId = this.employeeId;
          this.appraisalStandardsArray.push(this.appraisalStandards);
        }
      }
      // Excess Array
      for (let i = 0; i < this.excessArray.length; i++) {
        this.appraisalStandards = {};
        const index = this.appraisalStandardsArray.findIndex(x => x.DescriptionName === this.excessArray[i].description);
        if (index > -1) {
          this.appraisalStandardsArray[index].IsExceeds = true;
        } else {
          this.appraisalStandards.DescriptionName = this.excessArray[i].description;
          this.appraisalStandards.IsExceeds = true;
          this.appraisalStandards.IsBelow = false;
          this.appraisalStandards.IsAchieves = false;
          this.appraisalStandards.EmployeeId = this.employeeId;
          this.appraisalStandardsArray.push(this.appraisalStandards);
        }
      }

      const data = {
        EmployeeId: this.employeeId,
        AppraisalType: Number(this.rForm.get('appraisalType').value),
        AppraisalDateFrom: moment(this.rForm.get('fromDate').value).format('YYYY-MM-DD'),
        AppraisalDateTo: moment(this.rForm.get('toDate').value).format('YYYY-MM-DD'),
        StandardList: this.appraisalStandardsArray
      };      
      this.empService.AddEmployeeAppraisalDetail(data).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: 'Details added successfully.', title: '' });
            this.rForm.reset();
            this.formDirective.resetForm();
            window.location.reload();
            this.getEmployeeAppraisalList();
            break;

          default:
            break;
        }
      });
    }
  }
  EditEmployeeAppraisalDetails() {
    this.AppraisalListdata = this.dataSourceStandard.filteredData;
    if (this.rForm.valid) {
      // for (let i = 0; i < this.EditAppraisalList.length; i++) {
      //   this.appraisalStandards = {};
      //   this.appraisalStandards.DescriptionName = this.EditAppraisalList[i].descriptionName;
      //   this.appraisalStandards.IsAchieves = this.EditAppraisalList[i].isAchieves;
      //   this.appraisalStandards.IsBelow = this.EditAppraisalList[i].isBelow;
      //   this.appraisalStandards.IsExceeds = this.EditAppraisalList[i].isExceeds;
      //   this.appraisalStandardsArray.push(this.appraisalStandards);
      // }
      console.log("acheivesArray",this.acheivesArray);
      console.log("excessArray",this.excessArray);
      console.log("belowArray",this.belowArray);

      for (let i = 0; i < this.acheivesArray.length; i++) {
        this.appraisalStandards = {};
        const index = this.appraisalStandardsArray.findIndex(x => x.DescriptionName === this.acheivesArray[i].description);
        if (index > -1) {
          this.appraisalStandardsArray[index].IsAchieves = true;
        } else {
          this.appraisalStandards.DescriptionName = this.acheivesArray[i].description;
          this.appraisalStandards.IsAchieves = true;
          this.appraisalStandards.IsBelow = false;
          this.appraisalStandards.IsExceeds = false;
          this.appraisalStandards.EmployeeId = this.EditAppraisalId;
          this.appraisalStandardsArray.push(this.appraisalStandards);
        }
      }
      // Acheives Array
      for (let i = 0; i < this.belowArray.length; i++) {
        this.appraisalStandards = {};
        const index = this.appraisalStandardsArray.findIndex(x => x.DescriptionName === this.belowArray[i].description);
        if (index > -1) {
          this.appraisalStandardsArray[index].IsBelow = true;
        } else {
          this.appraisalStandards.DescriptionName = this.belowArray[i].description;
          this.appraisalStandards.IsBelow = true;
          this.appraisalStandards.IsExceeds = false;
          this.appraisalStandards.IsAchieves = false;
          this.appraisalStandards.EmployeeId = this.EditAppraisalId;
          this.appraisalStandardsArray.push(this.appraisalStandards);
        }
      }
      // Excess Array
      for (let i = 0; i < this.excessArray.length; i++) {
        this.appraisalStandards = {};
        const index = this.appraisalStandardsArray.findIndex(x => x.DescriptionName === this.excessArray[i].description);
        if (index > -1) {
          this.appraisalStandardsArray[index].IsExceeds = true;
        } else {
          this.appraisalStandards.DescriptionName = this.excessArray[i].description;
          this.appraisalStandards.IsExceeds = true;
          this.appraisalStandards.IsBelow = false;
          this.appraisalStandards.IsAchieves = false;
          this.appraisalStandards.EmployeeId = this.EditAppraisalId;
          this.appraisalStandardsArray.push(this.appraisalStandards);
        }
      }
      console.log("appraisalStandardsArray",this.appraisalStandardsArray);
      const data = {
        Id: this.EditAppraisalId,
        AppraisalType: Number(this.rForm.get('appraisalType').value),
        AppraisalDateFrom: moment(this.rForm.get('fromDate').value).format('YYYY-MM-DD'),
        AppraisalDateTo: moment(this.rForm.get('toDate').value).format('YYYY-MM-DD'),
        StandardList: this.appraisalStandardsArray
      };
      this.empService.EditEmployeeAppraisalDetail(data).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: 'Details Updated successfully.', title: '' });
            this.rForm.reset();
            this.formDirective.resetForm();
           // window.location.reload();
           this.textName = 'Submit';
            this.getEmployeeAppraisalList();
            break;

          default:
            break;
        }
      });
    }
  }

  DeleteModal(appraisalID, _e) {

    this.deleteAppraisalId = appraisalID;
  }

  DeleteAppraisalInfo(event) {
    this.AppraisalInfoModel.Id = this.deleteAppraisalId;
    this.empService.DeleteAppraisalDetails(this.AppraisalInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.getEmployeeAppraisalList();
      }
      else {
        this.notificationService.Error({ message: data.message, title: null });
      }

    });
  }
  GetAppraisalType() {
    this.commonservice.getAppraisalType().subscribe((res => {
      if (res) {
        this.response = res;
        this.AppraisalTypeList = this.response.responseData || [];

      } else {

      }
    }));
  }
  getAppraisalCrieteria() {
   this.commonservice.getAppraisalCrieteria().subscribe((res: any) => {
     this.totalCount = res.total;
     this.response = res;
     debugger
     switch (this.response.status) {
       case 1:
         this.AppraisalcrieteriaList = this.response.responseData;
         this.dataSourceStandard = new MatTableDataSource(this.AppraisalcrieteriaList);
         break;
         default:
           this.dataSource = new MatTableDataSource(this.Appraisalcrieteriadata);
           break;
     }
   });
 }

 getTotalWarning(){
   debugger;
  const data = {
    EmployeeId: this.employeeId,
  }
  this.commonservice.getTotalWarning(data).subscribe((res=>{
    if(res){
      this.response = res;
      this.detailList=this.response.responseData||[];
      this.WarningData=this.detailList[0]['dateOfJoining']
     // this.datetoday= this.detailList[0]['todaydate']
     this.datetoday=this.todayDate;
      this.totalwarning=this.detailList[0]['totalwarning'];
     
    }else{

    }
  }));
}
}

export interface AllEmployeeAppraisalInfo {
  Id?: number;
}
