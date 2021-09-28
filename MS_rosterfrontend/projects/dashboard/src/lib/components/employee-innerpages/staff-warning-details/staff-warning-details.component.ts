import { Component, OnInit, ViewChild,ElementRef } from '@angular/core';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { EmpServiceService } from '../../../services/emp-service.service';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { EmployeeStaffWarning } from '../view-model/employee-staff-warning';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { Paging } from 'projects/viewmodels/paging';
import { MatTableDataSource } from '@angular/material/table';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import * as moment from 'moment';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MatSort } from '@angular/material/sort';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';


@Component({
  selector: 'app-staff-warning-details',
  templateUrl: './staff-warning-details.component.html',
  styleUrls: ['./staff-warning-details.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})
export class StaffWarningDetailsComponent implements OnInit {
  response: ResponseModel = {};
  getErrorMessage: 'Please Enter Value';
  rForm: FormGroup;
  rForm1: FormGroup;
  employeeId: any;
  paging: Paging = {};
  totalCount: number;
  staffwarningData:StaffWarningElement[];
  staffwarningList=[];
  StaffInfoModel: StaffWarningElementInfo = {};
  textName:string;
  deleteStaffId : number;
  responseModel: ResponseModel = {};
  OffenseTypeList:any;
  WarningTypeList:any;
  EditList: StaffWarningElementInfo[] = [];
  employeePrimaryInfo: {};
  orderBy: number;
  orderColumn: number;
   
  isShownOther: boolean = false ;
  @ViewChild('btnAddStaffwaringCancel') cancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('btnEditStaffwaringCancel') editCancel: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
displayedColumns: string[] = ['sr', 'warningType','offenseTypeName', 'description', 'improvementPlan','otheroffenses', 'warningDate','action'];
 dataSource = new MatTableDataSource(this.staffwarningList);
  StaffId: any;
  EditId: number;
  other: any;
  list: any;
  selectedType: any;
  selectedName: any;
  constructor(private fb: FormBuilder, private route: ActivatedRoute,private loaderService: LoaderService,
    private notificationService: NotificationService, private empService: EmpServiceService,private commonservice:CommonService) { 
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
   this.route.queryParams.subscribe(params => {
      // tslint:disable-next-line: radix
      this.employeeId = parseInt(params.Id);
      this.EditId = parseInt(params.EId);
    });
    this.createForm();
    this.EditcreateForm();
    this.employeeId > 0 ? this.getEmployeeInfo() : null;
    this.getEmployeestaffwarningList();
    this.getOffenseType();
    this.getWarningType()
    if(this.EditId>0){
      this.getstaffwraningById();
     }
 }

 ngAfterViewInit(): void {
  setTimeout(() => {
    this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap(() => this.getEmployeestaffwarningList())
      )
      .subscribe();
  }, 2000);

}
getSortingOrder() {
  const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
  this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
  switch (sortColumn) {
   
    case 'warningType':
      this.orderColumn = 0;
      break;
    case 'offenseTypeName':
      this.orderColumn = 1;
      break;
      case 'description':
        this.orderColumn = 2;
        break;
      case 'improvementPlan':
        this.orderColumn = 3;
        break;
        case 'otheroffenses':
          this.orderColumn = 4;
          break;
    case 'createdDate':
      this.orderColumn = 5;
      break;

    default:
      break;
  }
}
 getstaffwraningById(){
  const data = {
    Id: Number(this.EditId),
   
  };
  this.empService.getStaffwarningById(data).subscribe(res => {
    this.response = res;
       this.EditList = this.response.responseData;
       document.getElementById("openEditModalButton").click();
       this.StaffId=this.EditList[0]['id'];;
       this.rForm1.patchValue({
       EditwarningType:this.EditList[0]['warningType'], 
       EditoffensesType:this.EditList[0]['offensesType'], 
       Editdescription:this.EditList[0]['description'],
       EditimprovementPlan:this.EditList[0]['improvementPlan']
       });
       if(this.EditList[0]['otherOffenses']!=""){
        Editotheroffenses:this.EditList[0]['otherOffenses'],
        this.isShownOther=true;
      }
      else{
        this.isShownOther=false;
      }
    })
}
 
 getEmployeeInfo() {
    this.empService.getEmployeeDetails(this.employeeId).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.rForm.controls.empName.patchValue(this.responseModel.responseData.employeePrimaryInfo.firstName + ' ' +
            this.responseModel.responseData.employeePrimaryInfo.lastName);
          this.rForm.controls.employeeId.patchValue(this.responseModel.responseData.employeePrimaryInfo.employeeId);
         // this.rForm.controls.departmentName.patchValue(this.responseModel.responseData.employeeJobProfile.departmentId);
          break;

        default:
          break;
      }

    });
  }
  createForm() {
    this.rForm = this.fb.group({
      otheroffenses: ['', Validators.nullValidator],
      warningType: ['', Validators.required],
      offensesType : ['', Validators.required],
      description: ['', Validators.required],
      improvementPlan: ['', Validators.required],
      employeeId : [],
      empName: [],
    });
  }
  EditcreateForm() {
    this.rForm1 = this.fb.group({
      Editotheroffenses: ['', Validators.nullValidator],
      EditwarningType: ['', Validators.required],
      EditoffensesType : ['', Validators.required],
      Editdescription: ['', Validators.required],
      EditimprovementPlan: ['', Validators.required],
      EditemployeeId : [],
      EditempName: [],
    });
  }
 
  get otheroffenses() {
    return this.rForm.get('otheroffenses');
  }
  get warningType() {
    return this.rForm.get('warningType');
  }
  get offensesType() {
    return this.rForm.get('offensesType');
  }
  get description() {
    return this.rForm.get('description');
  }
  get improvementPlan() {
    return this.rForm.get('improvementPlan');
  }
 
  AddStaffWarning(){
  if (this.rForm.valid) {
      const data = {
        'EmployeeId': this.employeeId,
        'OtherOffenses'  : this.otheroffenses.value,
        'WarningType'  : parseInt(this.warningType.value),
        'OffensesType'  : parseInt(this.offensesType.value),
        'Description'  : this.description.value,
        'ImprovementPlan' : this.improvementPlan.value,
      };

      this.empService.addEmployeeStaffWarning(data).subscribe(res => {
        this.response = res;        
        switch (this.response.status) {
          case 1:
            this.notificationService.Success({ message: this.response.message, title: null });
            this.rForm.reset();
            this.formDirective.resetForm();
            this.getEmployeestaffwarningList();
            
            // alert('Details added successfully');
            break;

          default:
            this.notificationService.Warning({ message: this.response.message, title: null });
            break;
        }
      });
    }
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getEmployeestaffwarningList();
  }
  getEmployeestaffwarningList() {
    this.getSortingOrder();
     const data = {
     PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      EmployeeId : this.employeeId,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
   
      this.empService.getEmployeestaffwarningList(data).subscribe((res:any)=>{
        this.totalCount = res.total;
      if(res)
      {
        let staffarray = [];
       
        if(res.responseData!=null)
        {
        this.staffwarningList=res.responseData;
        
        this.staffwarningList.forEach(function(value){
          let Commdata={
          Id:value['id'],
          employeeId:value['employeeId'],
          employeeName: 
          value['firstName'] +
          ((value['middleName'] ===undefined || value['middleName'] === null)?'':' '+value['middleName']) 
          +
          ((value['lastName'] ===undefined || value['lastName'] === null)?'':' '+value['lastName']) ,
          warningType:value['warning'],
          managerName:value['managerName'],
          jobTitle:value['jobTitle'],
         // date:value['date'],
          description:value['description'],
          departmentName:value['departmentName'],
          offenseType:value['offensesType'],
          improvementPlan:value['improvementPlan'],
          warningval:value['warningType'],
          offenseTypeName:value['offensesTypeName'],
          createdDate:value['createdDate'],
          otherOffenses:value['otherOffenses'],
          ACTION:''
        }
        staffarray.push(Commdata);
        })
        this.staffwarningData=staffarray;
        this.dataSource.data = this.staffwarningData;
        
        }
        else{
          this.dataSource.data=staffarray;
         
         // this.noData = this.dataSource.connect().pipe(map(data => data.length === 0));
        }
    }
      else{
        
      }return this.dataSource.data;
      
    })
  }
  openEditDetails(elem) {
     document.getElementById("openEditModalButton").click();
    this.StaffId=elem.Id;
    this.rForm1.patchValue({
    EditwarningType: elem.warningval,
    EditoffensesType: elem.offenseType,
    Editdescription: elem.description,
    EditimprovementPlan: elem.improvementPlan,
    });
    debugger;
    if(elem.otherOffenses!=null && elem.otherOffenses!=""){
      this.isShownOther=true;
      this.rForm1.get('Editotheroffenses').patchValue(elem.otherOffenses);
     }
    else{
      this.isShownOther=false;
    }
  }
  UpdateStaffWarning(){
    if (this.rForm1.valid) {
          const data = {
            'Id': this.StaffId,
            'WarningType'  :this.rForm1.get('EditwarningType').value, 
            'OffensesType'  :this.rForm1.get('EditoffensesType').value, 
            'Description'  :this.rForm1.get('Editdescription').value, 
            'ImprovementPlan' : this.rForm1.get('EditimprovementPlan').value,
            'OtherOffenses' : this.rForm1.get('Editotheroffenses').value,
            
          };
    
          this.empService.UpdateEmployeeStaffWarning(data).subscribe(res => {
            this.response = res;
            switch (this.response.status) {
              case 1:
                this.notificationService.Success({ message: this.response.message, title: null });
                this.editCancel.nativeElement.click();
                this.getEmployeestaffwarningList();
               
                // alert('Details added successfully');
                break;
    
              default:
                this.notificationService.Success({ message: this.response.message, title: null });
                break;
            }
          });
    }
      }
      OnSubmit(){
        if(this.textName=="Submit"){
          this.AddStaffWarning();
        }
        else if(this.textName=="Update"){
        this.UpdateStaffWarning();
        }
      }
      DeleteModal(StaffID,_e)
      {
    
        this.deleteStaffId = StaffID;
      }
      
    DeletestaffInfo(event){
        this.StaffInfoModel.Id=this.deleteStaffId;
        this.empService.DeleteStaffDetails(this.StaffInfoModel).subscribe((data: any) => {
          if (data.status == 1) {
            this.notificationService.Success({ message: data.message, title: null });
            this.getEmployeestaffwarningList();
          }
          else {
            this.notificationService.Error({ message: data.message, title: null });
          }
    
        })
      }
      getEmployeeDetails() {

        this.empService.getEmployeeDetails(this.employeeId).subscribe(res => {
    
          this.responseModel = res;
          console.log('response', this.responseModel);
          if (this.responseModel.status > 0) {
           
    
          }
          else {
            
          }
        });
      }
      getWarningType(){
        this.commonservice.getWarningType().subscribe((res=>{
          if(res){
            this.response = res;
            this.WarningTypeList=this.response.responseData||[];
           
          }else{
    
          }
        }));
      }
      getOffenseType(){
        this.commonservice.getOffenseType().subscribe((res=>{
          if(res){
            this.response = res;
            this.OffenseTypeList=this.response.responseData||[];
           
          }else{
    
          }
        }));
      }
      selectChangeHandler(event:any) {
       this.list=this.OffenseTypeList;
        this.selectedType = event;
       const index = this.list.findIndex(x => x.id == this.selectedType);
        this.selectedName= this.list[index].codeDescription;
        if (this.selectedName == "Other") {
          this.isShownOther=true;
     }
     else{
       this.isShownOther=false;
       this.rForm.get('otheroffenses').patchValue("");

     }
      }
      selectChangeHandlerEdit(event:any) {
        this.list=this.OffenseTypeList;
         this.selectedType = event;
        const index = this.list.findIndex(x => x.id == this.selectedType);
         this.selectedName= this.list[index].codeDescription;
         if (this.selectedName == "Other") {
           this.isShownOther=true;
      }
      else{
        this.isShownOther=false;
        this.rForm1.get('Editotheroffenses').patchValue("");
 
      }
       }
      getOtheroffenses() {    
        if (this.selectedName == "Other") {
         this.isShownOther=true;
    }
    else{
      this.isShownOther=false;
    }
      }
      getEditOtheroffenses() {    
        if (this.selectedName == "Other") {
         this.isShownOther=true;
    }
    else{
      this.isShownOther=false;
    }
      }
}
export interface StaffWarningElementInfo {
  Id?:number
}
export interface StaffWarningElement {
  Id?:number
  employeeId?:number,
  employeeName: string;
  warningType: number;
  managerName: string;
  JobTitle: string;
 // date: string;
  description:string;
  departmentName:string;
  offenseType:number;
  improvementPlan:string;
  warningval:number;
}
