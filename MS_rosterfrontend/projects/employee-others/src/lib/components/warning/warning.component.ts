import { Component, OnInit, ViewChild,ElementRef } from '@angular/core';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { Paging } from 'projects/viewmodels/paging';
import { MatTableDataSource } from '@angular/material/table';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MatSort } from '@angular/material/sort';
import { OtherService } from '../../services/other.service';

@Component({
  selector: 'lib-warning',
  templateUrl: './warning.component.html',
  styleUrls: ['./warning.component.scss'],
 
})

export class WarningComponent implements OnInit {
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
  constructor(private fb: FormBuilder, private route: ActivatedRoute,private notificationService: NotificationService, 
    private otherService: OtherService,private commonservice:CommonService) { 
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.employeeId = params.params.id;
    });
    this.createForm();
    this.EditcreateForm();
    this.getEmployeestaffwarningList();
    this.getOffenseType();
    this.getWarningType()
    
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
  createForm() {
    this.rForm = this.fb.group({
      otheroffenses: ['', Validators.nullValidator],
      warningType: ['', Validators.required],
      offensesType : ['', Validators.required],
      description: ['', Validators.required],
      improvementPlan: ['', Validators.required],
      
    });
  }
  EditcreateForm() {
    this.rForm1 = this.fb.group({
      Editotheroffenses: ['', Validators.nullValidator],
      EditwarningType: ['', Validators.required],
      EditoffensesType : ['', Validators.required],
      Editdescription: ['', Validators.required],
      EditimprovementPlan: ['', Validators.required],
     
    });
  }
  openModal(open: boolean) {
    document.getElementById("openModalButton").click();
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
  
  AddEmployeeStaffWarning(){
    debugger;
     if (this.rForm.valid) {
      const data = {
        'EmployeeId': Number(this.employeeId),
        'OtherOffenses'  : this.otheroffenses.value,
        'WarningType'  : parseInt(this.warningType.value),
        'OffensesType'  : parseInt(this.offensesType.value),
        'Description'  : this.description.value,
        'ImprovementPlan' : this.improvementPlan.value,
      };

      this.otherService.addEmployeeStaffWarning(data).subscribe(res => {
        this.response = res;        
        switch (this.response.status) {
          case 1:
            this.notificationService.Success({ message: this.response.message, title: null });
            this.cancel.nativeElement.click();
            this.rForm.reset();
            this.formDirective.resetForm();
            this.getEmployeestaffwarningList();
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
    debugger;
    this.getSortingOrder();
     const data = {
     PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      EmployeeId : Number(this.employeeId),
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
     this.otherService.getEmployeestaffwarning(data).subscribe((res:any)=>{
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
  UpdateEmployeeStaffWarning(){
    if (this.rForm1.valid) {
          const data = {
            'Id': this.StaffId,
            'WarningType'  :this.rForm1.get('EditwarningType').value, 
            'OffensesType'  :this.rForm1.get('EditoffensesType').value, 
            'Description'  :this.rForm1.get('Editdescription').value, 
            'ImprovementPlan' : this.rForm1.get('EditimprovementPlan').value,
            'OtherOffenses' : this.rForm1.get('Editotheroffenses').value,
            
          };
    
          this.otherService.UpdateEmployeeStaffWarning(data).subscribe(res => {
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
     
      DeleteModal(StaffID,_e)
      {
    
        this.deleteStaffId = StaffID;
      }
      
      DeleteEmpstaffWarning(event){
        this.StaffInfoModel.Id=this.deleteStaffId;
        this.otherService.DeleteStaffDetails(this.StaffInfoModel).subscribe((data: any) => {
          if (data.status == 1) {
            this.notificationService.Success({ message: data.message, title: null });
            this.getEmployeestaffwarningList();
          }
          else {
            this.notificationService.Error({ message: data.message, title: null });
          }
    
        })
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
      Reset(){
        this.rForm.reset();
        this.formDirective.resetForm();
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
  description:string;
  departmentName:string;
  offenseType:number;
  improvementPlan:string;
  warningval:number;
}