import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm, FormControl } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { ClientService } from '../../services/client.service';
import * as moment from 'moment';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ClientFundingInfo } from '../../view-models/client-fundinginfo';
import { ClientfundingTypeInfo } from '../../view-models/client-fundingTypeInfo';
import { Paging } from 'projects/viewmodels/paging';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { FundType } from 'projects/lhs-service/src/lib/viewmodels/gender';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { merge, Observable, Subject, ReplaySubject } from 'rxjs';
import { tap, startWith, map, takeUntil, take } from 'rxjs/operators';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
// interface FundType {
//   value: string;
//   name: string;
// }

//For Funding Info
export interface PeriodicElementFundingInfo {
  Id: number;
  fundType: string;
  amount: string;
  hours: number;
  expiry: string;
}

//For Funding Info
const ELEMENT_DATA_FUNDING_INFO: PeriodicElementFundingInfo[] = [
  { Id: 1, fundType: 'Fund Type', amount: '$2,500', hours: 11.5, expiry: '10-May-2020' },
  { Id: 2, fundType: 'Fund Type', amount: '$1,150', hours: 9.5, expiry: '12-Jun-2020' },
];
@Component({
  selector: 'lib-client-fundinginfo',
  templateUrl: './client-fundinginfo.component.html',
  styleUrls: ['./client-fundinginfo.component.css'],
  providers: [
    {
      provide: DateAdapter, useClass: AppDateAdapter
    },
    {
      provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
  ]
})
export class ClientFundinginfoComponent implements OnInit {
  getErrorMessage: 'Please Enter Value';


  //For Fund Type
  // ftypes: FundType[] = [
  //   { value: '', name: 'Select FundType' },
  //   { value: '1', name: 'Fund Type 1' },
  //   { value: '2', name: 'Fund Type 2' },
  //   { value: '3', name: 'Fund Type 3' }
  // ];

  ftypes: FundType[] = [];
  dataSourceAgreement: any;
  displayedColumnsFundingInfo: string[] = ['serviceType', 'paymentterm', 'nodays', 'payer', 'ammount', 'totalAmount', 'action'];
  displayedColumnsAgreement: string[] = ['claimNumber', 'fundTypeName', 'startDate', 'endDate', 'amount', 'refNumber', 'action'];
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  deletefundintypeId: any;
  totalCount: number;
  // dataSourceFundingInfo: any;
  @Input() ClientFundingInfo: ClientFundingInfo;
  @Input() ClientFundingmodel: ClientFundingInfo[];
  @Input() ClientfundingTypeInfo: ClientfundingTypeInfo;
  fundtypeInfo: ClientfundingTypeInfo[];
  ServiceData: ServiceDetails[];
  rForm: FormGroup;
  rFormAdd: FormGroup;
  rFormfundtype: FormGroup;
  rFormEditfundtype: FormGroup;
  response: ResponseModel = {};
  paging: Paging = {};
  FundingtypeList = [];
  FundingtypeModel: FundingTypedata = {};
  @ViewChild('btnclientfundinInfo') cancel: ElementRef;
  @ViewChild('btnclientupdate') update: ElementRef;
  @ViewChild('btnAgreementcancel') cancelagreement: ElementRef;
  @ViewChild('btnAgreementupdate') Addagreement: ElementRef;
  @ViewChild('btnAddfundtype') cancelAdd: ElementRef;
  @ViewChild('btnEditfundtype') cancelEdit: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  orderBy: number;
  orderColumn: number;
  isShown: boolean = false;
  isShownother: boolean = false;
  EditFundID: any;
  ClientId: number;
  responseModel: ResponseModel = {};
  ServiceTypeList: any;
  ServiceRate: any;
  RateId: any;
  Rate: any;
  Days: any;
  clientInfo: any;
  list: any[];
  selectedType: Event;
  selectedName: any;
  PaymentList: any;
  PayerList: any;
  isShownpayers: boolean = false;
  isShownpayersEdit: boolean = false;
  DaysText: any;
  DaysTextEdit: string;
  public control: FormControl = new FormControl();
  public searchcontrol: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild('Select') select: MatSelect;
  totalCountAgreement: number;
  AgreementId: any;
  deleteAgreementId: any;
  ReferenceList: any;
  constructor(private route: ActivatedRoute, private fb: FormBuilder, private clientservice: ClientService,
    private notificationService: NotificationService, private commonService: CommonService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }
  dataSourceFundingInfo = new MatTableDataSource(this.FundingtypeList);
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.ClientId = parseInt(params['Id']);
      this.isShown = false;

    });

    this.getFundType();
    this.createForm();
    this.createFormAddAreement();
    this.createFormFundtype();
    this.EditFundtype();
    this.getClientDetails();
    this.GetList();
    this.getServiceTypelist();
    this.getPrimaryInfo();
    this.getPaymenttermlist();
    this.getreferencenumber();
    this.DaysText = "No. of Days";
    this.searchservicetype();
    this.searchserviceEdit();
  }
  searchservicetype() {
    this.control.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterServicetype();
      });
  }
  searchserviceEdit() {
    this.searchcontrol.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterServiceEdit();
      });
  }
  private filterServicetype() {
    if (!this.ServiceData) {
      return;
    }
    let search = this.control.value;
    if (!search) {
      this.filteredRecords.next(this.ServiceData.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
      this.filteredRecords.next(
        this.ServiceData.filter(ServiceType => ServiceType.supportItemName.toLowerCase().indexOf(search) > -1)
      );
    }
  }
  private filterServiceEdit() {
    if (!this.ServiceData) {
      return;
    }
    let search = this.searchcontrol.value;
    if (!search) {
      this.filteredRecords.next(this.ServiceData.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
      this.filteredRecords.next(
        this.ServiceData.filter(ServiceType => ServiceType.supportItemName.toLowerCase().indexOf(search) > -1)
      );
    }
  }
  // ngAfterViewInit(): void {
  //   setTimeout(() => {
  //     this.dataSourceFundingInfo !== undefined ? this.dataSourceFundingInfo.sort = this.sort : this.dataSourceFundingInfo;
  //     this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
  //     merge(this.sort.sortChange, this.paginator.page)
  //       .pipe(
  //         tap(() => this.GetList())
  //       )
  //       .subscribe();
  //   }, 2000);

  // }
  getFundType() {
    this.commonService.getFundType().subscribe(res => {
      this.responseModel = res;
      if (this.responseModel.status > 0) {
        this.ftypes = this.responseModel.responseData;
        this.ftypes.forEach(x => x.value = x.id);
      }
      else {
        this.notificationService.Warning({ message: this.response.message, title: null });
      }
    });
  }
  getother(event) {
    this.list = this.ftypes;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedName = this.list[index].codeDescription;
    if (this.selectedName == "Other") {
      this.isShown = true;
    }
    else {
      this.isShown = false;
      this.rForm.get('Other').patchValue("");
    }
  }

  getClientDetails() {
    const data = {
      ClientId: this.ClientId,
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize

    };
    this.clientservice.GetClientAgreementInfo(data).subscribe(res => {
      this.response = res;
      this.totalCountAgreement = this.response.total;
      debugger
      switch (this.response.status) {
        case 1:
          this.ClientFundingmodel = this.response.responseData;
          this.dataSourceAgreement = new MatTableDataSource(this.ClientFundingmodel);
          break;
        case 0:
          // this.notificationService.Warning({ message: this.response.message, title: null });
          this.dataSourceAgreement = new MatTableDataSource([]);
          break;
        default:
          break;
      }
    });

  }
  createForm() {
    this.rForm = this.fb.group({
      Fundtype: [null, Validators.required],
      RefNumber: [null, Validators.required],
      Other: [null, Validators.nullValidator],
      StartDate: [null, Validators.required],
      EndDate: [null, Validators.required],
      Amount: [null, Validators.required],
      ClaimNumber: [null, Validators.nullValidator],
    });
  }
  createFormAddAreement() {
    this.rFormAdd = this.fb.group({
      AddFundtype: [null, Validators.required],
      AddRefNumber: [null, Validators.required],
      AddOther: [null, Validators.nullValidator],
      AddStartDate: [null, Validators.required],
      AddEndDate: [null, Validators.required],
      AddAmount: [null, Validators.required],
      AddClaimNumber: [null, Validators.nullValidator],
    });
  }
  createFormFundtype() {
    this.rFormfundtype = this.fb.group({
      ServiceType: [null, Validators.required],
      Amount: [null, Validators.required],
      NoDays: [null, Validators.required],
      TotalAmount: [null, Validators.required],
      Paymentterm: [null, Validators.required],
      ReferenceNumber: [null, Validators.required],
      Payers: [null, Validators.nullValidator],
    });
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.GetList();
  }
  PageIndexEventAgreement(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getClientDetails();
  }
  EditFundtype() {
    this.rFormEditfundtype = this.fb.group({
      ServiceTypeEdit: [null, Validators.required],
      AmountEdit: [null, Validators.required],
      NoDaysEdit: [null, Validators.required],
      TotalAmountEdit: [null, Validators.required],
      PaymenttermEdit: [null, Validators.required],
      ReferenceNumberEdit: [null, Validators.required],
      PayersEdit: [null, Validators.nullValidator],
    });
  }
  getPrimaryInfo() {
    const data = {
      id: this.ClientId
    }
    this.clientservice.getClientPrimaryInfo(data).subscribe((res: any) => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.clientInfo = this.responseModel.responseData;
          this.rForm.controls['ClaimNumber'].patchValue(this.clientInfo.clientId);
          this.rForm.get('ClaimNumber').disable();
          this.rFormAdd.controls['AddClaimNumber'].patchValue(this.clientInfo.clientId);
          this.rFormAdd.get('AddClaimNumber').disable();
          break;
        case 0:
          this.notificationService.Error({ message: 'some Error occured', title: null });
          break;
        default:

          break;
      }
    })
  }

  AddAgreementInfo() {
    if (this.rFormAdd.valid) {
      const data = {
        ClientId: this.ClientId,
        FundType: this.rFormAdd.value.AddFundtype,
        RefNumber: this.rFormAdd.value.AddRefNumber,
        Other: this.rFormAdd.value.AddOther,
        Amount: (this.rFormAdd.value.AddAmount),
        StartDate: moment(this.rFormAdd.value.AddStartDate).format('YYYY-MM-DD'),
        EndDate: moment(this.rFormAdd.value.AddEndDate).format('YYYY-MM-DD'),
        ClaimNumber: (this.rFormAdd.controls.AddClaimNumber.value),
      }
      this.clientservice.AddClientFundingInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.ClientFundingInfo = this.response.responseData;
            this.cancelagreement.nativeElement.click();
            this.notificationService.Success({ message: "Details Added Successfully", title: null });
            this.getClientDetails();
            this.getreferencenumber();
            this.rFormAdd.reset();
            this.formDirective.resetForm();
            break;

          default:
            this.notificationService.Error({ message: this.response.message, title: null });
            break;
        }
      });
    }
    else {

    }
  }
  UpdateFundingInfo() {
    if (this.rForm.valid) {
      const data = {
        Id: this.AgreementId,
        ClientId: this.ClientId,
        FundType: this.rForm.value.Fundtype,
        RefNumber: this.rForm.value.RefNumber,
        Other: this.rForm.value.Other,
        Amount: (this.rForm.value.Amount),
        StartDate: moment(this.rForm.value.StartDate).format('YYYY-MM-DD'),
        EndDate: moment(this.rForm.value.EndDate).format('YYYY-MM-DD'),
        ClaimNumber: (this.rForm.controls.ClaimNumber.value),
      }

      this.clientservice.AddClientFundingInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.ClientFundingInfo = this.response.responseData;
            this.cancel.nativeElement.click();
            this.notificationService.Success({ message: "Details Updated Successfully", title: null });
            this.getClientDetails();
            break;

          default:
            this.notificationService.Error({ message: this.response.message, title: null });
            break;
        }
      });
    }
    else {

    }
  }

  AddFuntypeDetails() {
    if (this.totalCountAgreement > 0) {
      if (this.rFormfundtype.valid) {
        if (this.selectedName == "Food" || this.selectedName == "Rent" || this.selectedName == "Utilities") {
          if (this.rFormfundtype.controls.Payers.value == null || this.rFormfundtype.controls.Payers.value) {
            this.notificationService.Warning({ message: "Please Select Payer", title: null });
            return;
          }
        }
        else {

        }
        const data = {
          ClientId: this.ClientId,
          ServiceType: this.rFormfundtype.value.ServiceType,
          Ammount: (this.rFormfundtype.controls.Amount.value),
          NoDays: parseInt(this.rFormfundtype.value.NoDays),
          TotalAmount: Number(this.rFormfundtype.controls.TotalAmount.value),
          ClaimNumber: this.clientInfo.clientId,
          PaymentTerm: (this.rFormfundtype.controls.Paymentterm.value),
          ReferenceNumber: (this.rFormfundtype.controls.ReferenceNumber.value),
          Payer: (this.rFormfundtype.controls.Payers.value),
        }

        this.clientservice.AddClientFundingTypeInfo(data).subscribe(res => {
          this.response = res;
          switch (this.response.status) {
            case 1:
              this.ClientfundingTypeInfo = this.response.responseData;
              this.cancelAdd.nativeElement.click();
              this.notificationService.Success({ message: this.response.message, title: null });
              this.rFormfundtype.reset();
              this.formDirective.resetForm();
              this.GetList();
              break;
            default:
              this.notificationService.Error({ message: this.response.message, title: null });
              break;
          }
        });
      }
    }
    else {
      this.notificationService.Warning({ message: "Please add Client's Agreement details before adding Services", title: null });

    }

  }

  UpdateFuntypeDetails() {
    if (this.rFormEditfundtype.valid) {
      const data = {
        Id: this.EditFundID,
        ServiceType: this.rFormEditfundtype.value.ServiceTypeEdit,
        Ammount: (this.rFormEditfundtype.controls.AmountEdit.value),
        NoDays: (this.rFormEditfundtype.controls.NoDaysEdit.value),
        TotalAmount: Number(this.rFormEditfundtype.controls.TotalAmountEdit.value),
        ClaimNumber: this.clientInfo.clientId,
        PaymentTerm: Number(this.rFormEditfundtype.controls.PaymenttermEdit.value),
        ReferenceNumber: Number(this.rFormEditfundtype.controls.ReferenceNumberEdit.value),
        Payer: Number(this.rFormEditfundtype.controls.PayersEdit.value),
      }

      this.clientservice.EditClientFundingTypeInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.ClientfundingTypeInfo = this.response.responseData;
            this.cancelEdit.nativeElement.click();
            this.notificationService.Success({ message: this.response.message, title: null });
            this.GetList();
            break;
          default:
            this.notificationService.Error({ message: this.response.message, title: null });
            break;
        }
      });
    }
    else {
      this.notificationService.Warning({ message: "Please Add Funding Info First", title: null });
    }
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'serviceType':
        this.orderColumn = 0;
        break;
      case 'nodays':
        this.orderColumn = 1;
        break;
      case 'ammount':
        this.orderColumn = 2;
        break;
      case 'totalAmount':
        this.orderColumn = 3;
        break;

      case 'createdDate':
        this.orderColumn = 4;
        break;

      default:
        break;
    }
  }
  GetList() {
    // this.getSortingOrder();
    const data = {
      ClientId: this.ClientId,
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.clientservice.GetClientFundingTypeInfo(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;

      let fundtypearray = [];
      if (this.response.responseData != null) {
        this.FundingtypeList = this.response.responseData;
        this.FundingtypeList.forEach(function (value) {
          let Commdata = {
            Id: value['id'],
            ammount: value['ammount'],
            noDays: value['noDays'],
            totalAmount: value['totalAmount'],
            serviceType: value['serviceType'],
            serviceTypeName: value['serviceTypeName'],
            paymentTerm: value['paymentTerm'],
            paymentTermName: value['paymentTermName'],
            payer: value['payer'],
            payerName: value['payerName'],
            referenceNumber: value['referenceNumber'],
            ACTION: ''
          }
          fundtypearray.push(Commdata);
        })
        this.fundtypeInfo = fundtypearray;
        this.dataSourceFundingInfo.data = this.fundtypeInfo;

      }
      else {
        this.dataSourceFundingInfo.data = fundtypearray;

      }

    });
  }

  DeleteModal(fundingtypeId, _e) {

    this.deletefundintypeId = fundingtypeId;
  }

  DeletefundingTypeInfo(event) {
    this.FundingtypeModel.Id = this.deletefundintypeId;
    this.clientservice.DeleteClientFundingTypeInfo(this.FundingtypeModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.GetList();

      }
      else {
        this.notificationService.Error({ message: data.message, title: null });
      }

    })
  }
  DeleteModalAgreement(AgreementtypeId, _e) {
    this.deleteAgreementId = AgreementtypeId;
  }

  DeleteAgreementInfo(event) {
    const data = {
      Id: this.deleteAgreementId
    }
    this.clientservice.DeleteClientAgreement(data).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.getClientDetails();
      }
      else {
        this.notificationService.Error({ message: data.message, title: null });
      }

    })
  }
  getServiceTypelist() {
    this.commonService.getServiceList().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.ServiceTypeList = this.responseModel.responseData || [];
        this.ServiceData = this.ServiceTypeList;
        this.filteredRecords.next(this.ServiceData.slice());
      } else {

      }
    }));
  }

  getPaymenttermlist() {
    this.commonService.getPaymentterm().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.PaymentList = this.responseModel.responseData || [];

      } else {

      }
    }));
  }
  getreferencenumber() {
    const data = {
      ClientId: this.ClientId
    }
    this.commonService.getReferenceNumber(data).subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.ReferenceList = this.responseModel.responseData || [];

      } else {

      }
    }));
  }
  getPayerlist() {
    this.commonService.getPayers().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.PayerList = this.responseModel.responseData || [];

      } else {

      }
    }));
  }
  getServiceRate(Id: number) {
    this.commonService.getServicerate(Id).subscribe((res => {
      if (res) {
        this.response = res;
        this.ServiceRate = this.response.responseData || {};
        if (this.ServiceRate.rate > 0) {
          this.rFormfundtype.get('Amount').setValue(this.ServiceRate.rate);
          //this.rFormfundtype.get('Amount').disable();
        }
        else {
          this.rFormfundtype.get('Amount').setValue("");
          this.rFormfundtype.get('Amount').enable();
        }
      } else {

      }
    }));
  }
  getServiceRateEdit(Id: number) {
    this.commonService.getServicerate(Id).subscribe((res => {
      if (res) {
        this.response = res;
        this.ServiceRate = this.response.responseData || {};
        if (this.ServiceRate.rate > 0) {
          this.rFormEditfundtype.get('AmountEdit').setValue(this.ServiceRate.rate);
          //this.rFormEditfundtype.get('AmountEdit').disable();
        }
        else {
          this.rFormEditfundtype.get('AmountEdit').setValue("");
          this.rFormEditfundtype.get('AmountEdit').enable();
        }
      } else {

      }
    }));
  }
  getRate(event) {
    this.RateId = this.rFormfundtype.value.ServiceType,
      this.getServiceRate(this.RateId);
    this.list = this.ServiceTypeList;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedName = this.list[index].supportItemName;
    if (this.selectedName == "Food" || this.selectedName == "Rent" || this.selectedName == "Utilities") {
      this.getPayerlist();
      this.isShownpayers = true;
    }
    else {
      this.isShownpayers = false;
    }

  }
  getRateEdit(event) {

    this.RateId = this.rFormEditfundtype.value.ServiceTypeEdit,
      this.getServiceRateEdit(this.RateId);
    this.list = this.ServiceTypeList;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedName = this.list[index].supportItemName;
    if (this.selectedName == "Food" || this.selectedName == "Rent" || this.selectedName == "Utilities") {
      this.getPayerlist();
      this.isShownpayersEdit = true;
    }
    else {
      this.isShownpayersEdit = false;
      this.rFormEditfundtype.get('PayersEdit').setValue(0);
    }
  }
  gettotalAmount() {

    this.Rate = this.rFormfundtype.controls.Amount.value,
      this.Days = this.rFormfundtype.controls.NoDays.value
    this.rFormfundtype.get('TotalAmount').setValue(((this.Rate) * (this.Days)).toFixed(2));
    // this.rFormfundtype.get('TotalAmount').disable();
  }
  gettotalAmountEdit() {

    this.Rate = this.rFormEditfundtype.controls.AmountEdit.value,
      this.Days = this.rFormEditfundtype.controls.NoDaysEdit.value
    this.rFormEditfundtype.get('TotalAmountEdit').setValue(((this.Rate) * (this.Days)).toFixed(2));
    //this.rFormEditfundtype.get('TotalAmountEdit').disable();
  }
  openEditAgreement(element) {
    debugger
    document.getElementById("openEditModalButtonAgreement").click();
    this.AgreementId = element.id;
    this.rForm.get('Fundtype').patchValue(element.fundType);
    this.rForm.get('RefNumber').patchValue(element.refNumber);
    this.rForm.get('StartDate').patchValue(element.startDate);
    this.rForm.get('EndDate').patchValue(element.endDate);
    this.rForm.get('Amount').patchValue(element.amount);
    if (element.other != "" && element.other != null) {
      this.rForm.get('Other').patchValue(element.other);
      this.isShown = true;
    }
    else {
      this.isShown = false;

    }
  }
  openEditDetailsfunding(elem) {
    document.getElementById("openEditModalButtonfund").click();
    this.list = this.ServiceTypeList;
    this.selectedType = elem.serviceType;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedName = this.list[index].supportItemName;
    if (this.selectedName == "Food" || this.selectedName == "Rent" || this.selectedName == "Utilities") {
      this.getPayerlist();
      this.rFormEditfundtype.controls['PayersEdit'].setValue((elem.payer));
      this.isShownpayersEdit = true;
      this.rFormEditfundtype.controls['AmountEdit'].setValue(elem.ammount);
      this.rFormEditfundtype.get('TotalAmountEdit').enable();
    }
    else {
      this.isShownpayersEdit = false;
      this.rFormEditfundtype.controls['AmountEdit'].setValue(elem.ammount);
      // this.rFormEditfundtype.get('TotalAmountEdit').disable();
    }
    this.EditFundID = elem.Id;
    this.rFormEditfundtype.controls['ServiceTypeEdit'].setValue(Number(elem.serviceType));
    this.rFormEditfundtype.controls['NoDaysEdit'].setValue(elem.noDays);
    this.rFormEditfundtype.controls['TotalAmountEdit'].setValue(elem.totalAmount);

    this.rFormEditfundtype.controls['PaymenttermEdit'].setValue(Number(elem.paymentTerm));
    this.rFormEditfundtype.controls['ReferenceNumberEdit'].setValue(Number(elem.referenceNumber));
    this.list = this.PaymentList;
    this.selectedType = elem.paymentTerm;
    const index1 = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedName = this.list[index1].codeDescription;
    if (this.selectedName == "Daily") {
      this.DaysTextEdit = "No. of Days";
    }
    else if (this.selectedName == "Weekly") {
      this.DaysTextEdit = "No. of Weeks";
    }
    else if (this.selectedName == "Fortnightly") {
      this.DaysTextEdit = "No. of FortNight";
    }
    else if (this.selectedName == "Hourly") {
      this.DaysTextEdit = "No. of Hours";
    }
    else if (this.selectedName == "Monthly") {
      this.DaysTextEdit = "No. of Months";
    }
  }
  ResetForm() {
    this.rFormfundtype.reset();
    this.formDirective.resetForm();
  }
  Changetext(event) {
    this.list = this.PaymentList;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedName = this.list[index].codeDescription;

    if (this.selectedName == "Daily") {
      this.DaysText = "No. of Days";
    }
    else if (this.selectedName == "Weekly") {
      this.DaysText = "No. of Weeks";
    }
    else if (this.selectedName == "Fortnightly") {
      this.DaysText = "No. of FortNight";
    }
    else if (this.selectedName == "Hourly") {
      this.DaysText = "No. of Hours";
    }
    else if (this.selectedName == "Monthly") {
      this.DaysText = "No. of Months";
    }
  }
  ChangetextEdit(event) {
    this.list = this.PaymentList;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedName = this.list[index].codeDescription;

    if (this.selectedName == "Daily") {
      this.DaysTextEdit = "No. of Days";
    }
    else if (this.selectedName == "Weekly") {
      this.DaysTextEdit = "No. of Weeks";
    }
    else if (this.selectedName == "Fortnightly") {
      this.DaysTextEdit = "No. of FortNight";
    }
    else if (this.selectedName == "Hourly") {
      this.DaysTextEdit = "No. of Hours";
    }
    else if (this.selectedName == "Monthly") {
      this.DaysTextEdit = "No. of Months";
    }
  }
}
export interface FundingTypedata {
  Id?: number;
}
export interface ServiceDetails {
  Id: number;
  supportItemName: string,

}