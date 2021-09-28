import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { ClientService } from '../../services/client.service';
import { ClientPrimarycarerInfo } from '../../view-models/client-primary-carerinfo';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, from } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { MatTableDataSource } from '@angular/material/table';
interface RelationShip {
  id: number;
  codeDescription: string;
}
@Component({
  selector: 'lib-client-primarycareerinfo',
  templateUrl: './client-primarycareerinfo.component.html',
  styleUrls: ['./client-primarycareerinfo.component.scss']
})

export class ClientPrimaryCareerInfoComponent implements OnInit, OnChanges {
  @Input() clientcontactInfo: ClientPrimarycarerInfo[];
  @Input() primarycarerInfo: ClientPrimarycarerInfo;
  rForm: FormGroup;
  rFormInfo: FormGroup;
  response: ResponseModel = {};
  @ViewChild('btnprimarycareCancel') cancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  getErrorMessage:'Please Enter Value';
  relationShipList: RelationShip[];
  orderBy: number;
  orderColumn: number;
  @ViewChild('formDirective1') private formDirective1: NgForm;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  deleteexpId: any;
  totalCount: number;
  paging: Paging = {};
  contactotherList = [];
  ClientId: number;
  responseModel: ResponseModel = {};
  dataSourcecontactinfo: any;
  displayedColumnscontact: string[] = ['name', 'relationship', 'email', 'contactNo', 'phoneNo','otherRelation','action'];
  EditContactId: any;
  @ViewChild('btnprimarycareCancelEdit') editCancel: ElementRef;
  selectedType: any;
  list: RelationShip[];
  selectedName: string;
  isShownrelation: boolean = false ;
  GenderList: any;
  constructor(private route: ActivatedRoute,private commonService: CommonService, private fb: FormBuilder, private clientservice:ClientService, private notificationService: NotificationService) { }

  ngOnChanges(changes: SimpleChanges): void {
     this.route.queryParams.subscribe(params => {
        this.ClientId = parseInt(params['Id']);
      });
    }
  

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.ClientId = parseInt(params['Id']);
    });
    this.createFormInfo();
    this.createForm();
    this.getClientDetails();
    this.getrelationShip();
    this.GetGender();
   this.isShownrelation=false;
   
  }

  ngAfterViewInit(): void {
   
    setTimeout(() => {
      this.dataSourcecontactinfo !== undefined ? this.dataSourcecontactinfo.sort = this.sort : this.dataSourcecontactinfo;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getClientDetails())
        )
        .subscribe();
    }, 2000);

  }
  GetGender(){
    this.commonService.getGenderList().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.GenderList=this.responseModel.responseData||[];
       
      }else{

      }
    }));
  }
  getrelationShip(){
    
    this.commonService.getRelationShip().subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
         this.relationShipList = this.response.responseData;
          break;

        default:
          break;
      }
    });
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'name':
        this.orderColumn = 0;
        break;
      case 'relationship':
        this.orderColumn = 1;
        break;
      case 'email':
        this.orderColumn = 2;
        break;
        case 'contactNo':
          this.orderColumn = 3;
          break;
          case 'phoneNo':
          this.orderColumn = 4;
          break;
      case 'createdDate':
        this.orderColumn = 5;
        break;

      default:
        break;
    }
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getClientDetails();
  }
  getClientDetails() {
    this.getSortingOrder();
    const data = {
      ClientId: this.ClientId,
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy

    };
    this.clientservice.GetClientContactOtherPerson(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      
      let fundtypearray = [];
      if (this.response.responseData) {
        this.clientcontactInfo = this.response.responseData;
        this.dataSourcecontactinfo = new MatTableDataSource(this.clientcontactInfo);
         }
      else {
        this.dataSourcecontactinfo = new MatTableDataSource(this.contactotherList);
      }

    });
  }
  
  createForm() {
    this.rForm = this.fb.group({
      Name: [null, Validators.required],
      RelationShip: ['', Validators.required],
      EmailId: [null, Validators.compose([Validators.required, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      MobileNo: [null,[Validators.maxLength(16),Validators.required]],
      PhoneNo:[null,[Validators.maxLength(16),Validators.nullValidator]],
      MiddleName: [null, Validators.nullValidator],
      LastName: [null, Validators.required],
      otherrelation: [null, Validators.nullValidator],
      gender: [null, Validators.required],
    });
  }
  createFormInfo() {
    this.rFormInfo = this.fb.group({
      EditName: [null, Validators.required],
      EditMiddleName:[null, Validators.nullValidator],
      EditLastName: [null, Validators.required],
      EditRelationShip: [null, Validators.required],
      EditEmailId: [null, Validators.compose([Validators.required, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      EditMobileNo: ['',[Validators.maxLength(16),Validators.required]],
      EditPhoneNo: ['',[Validators.maxLength(16),Validators.nullValidator]],
      otherrelationEdit: [null, Validators.nullValidator],
      Editgender: [null, Validators.required],
    });
  }
  
  openEditModal(data: any) {  
    
    document.getElementById("openEditModalButton").click();
    this.EditContactId=data.id;
    this.rFormInfo.controls['EditName'].patchValue(data.firstName);
    this.rFormInfo.controls['EditMiddleName'].patchValue(data.middleName);
    this.rFormInfo.controls['EditLastName'].patchValue(data.lastName);
    this.rFormInfo.controls['EditRelationShip'].patchValue(data.relationShip);
    this.rFormInfo.controls['EditEmailId'].patchValue(data.email);
    this.rFormInfo.controls['EditMobileNo'].patchValue(data.contactNo);
    this.rFormInfo.controls['EditPhoneNo'].patchValue(data.phoneNo);
    this.rFormInfo.controls['Editgender'].patchValue(data.gender);
    if(data.otherRelation!=null && data.otherRelation!=""){
      this.rFormInfo.controls['otherrelationEdit'].patchValue(data.otherRelation);
      this.isShownrelation=true;
    }
    else{
      this.isShownrelation=false;
    }
   
  }
 AddPrimaryCareInfo() {
  
    if (this.rForm.valid) {
      const data = {
        ClientId: this.ClientId,
        FirstName: this.rForm.value.Name,
        RelationShip: this.rForm.value.RelationShip,
        Email: this.rForm.value.EmailId,
        ContactNo: String(this.rForm.value.MobileNo),
        PhoneNo:String(this.rForm.value.PhoneNo),
        MiddleName: this.rForm.value.MiddleName,
        LastName: this.rForm.value.LastName,
        OtherRelation: this.rForm.value.otherrelation,
        Gender: this.rForm.value.gender,
      }
     
      this.clientservice.AddPrimaryCareInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.primarycarerInfo = this.response.responseData;
            this.cancel.nativeElement.click();
            this.getClientDetails();
            this.rForm.reset();
            this.formDirective.resetForm();
            this.notificationService.Success({ message: this.response.message, title: null });
            break;

          default:
            this.notificationService.Error({ message: this.response.message, title: null });
            break;
        }
      });
    }
  }
  EditPrimaryCareInfo() {
    if (this.rFormInfo.valid) {
       const data = {
         Id:this.EditContactId,
         ClientId: this.ClientId,
         FirstName: this.rFormInfo.value.EditName,
         RelationShip: this.rFormInfo.value.EditRelationShip,
         Email: this.rFormInfo.value.EditEmailId,
         ContactNo: String(this.rFormInfo.value.EditMobileNo),
         PhoneNo:String(this.rFormInfo.value.EditPhoneNo),
         MiddleName: this.rFormInfo.value.EditMiddleName,
         LastName: this.rFormInfo.value.EditLastName,
         OtherRelation: this.rFormInfo.value.otherrelationEdit,
         Gender: this.rFormInfo.value.Editgender,
       }
      
       this.clientservice.AddPrimaryCareInfo(data).subscribe(res => {
         this.response = res;
         switch (this.response.status) {
           case 1:
             this.primarycarerInfo = this.response.responseData;
             this.editCancel.nativeElement.click();
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
  openModal(open: boolean) {
    document.getElementById("openAddWorkButton").click();
  }
  ResetForm(){
    this.rForm.reset();
    this.formDirective.resetForm();
  }
  DeleteModal(contactID,_e)
  {

    this.deleteexpId = contactID;
  }

  DeleteClientContactOtherPerson(event) {
    const data = {
      id: this.deleteexpId
    }
    this.clientservice.DeleteClientContactOtherPerson(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
         // this.dataSourceEducation.data = this.dataSourceEducation.data.filter(i => i !== elm)
          this.notificationService.Success({ message: this.response.message , title: '' })
         this.getClientDetails();
          break;
        case 0:
          this.notificationService.Error({ message: this.response.message, title: '' });
        default:
          break;
      }
    });
  }
  selectChangeHandler(event:any) {
    this.list=this.relationShipList;
     this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
     this.selectedName= this.list[index].codeDescription;
     if (this.selectedName == "Other") {
       this.isShownrelation=true;
  }
  else{
    this.isShownrelation=false;
    this.rForm.get('otherrelation').patchValue("");
    this.rFormInfo.get('otherrelationEdit').patchValue("");
  }

   }
}