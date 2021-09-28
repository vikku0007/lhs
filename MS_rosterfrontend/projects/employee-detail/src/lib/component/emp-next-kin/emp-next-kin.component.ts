import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { EmployeeKinInfo } from '../../view-models/employee-kin-info';
import { EmpDetailService } from '../../services/emp-detail.service';

interface RelationShip {
  id: number;
  codeDescription: string;
}
@Component({
  selector: 'lib-emp-next-kin',
  templateUrl: './emp-next-kin.component.html',
  styleUrls: ['./emp-next-kin.component.scss'],
  
})
export class EmpNextKinComponent implements OnInit, OnChanges {
  @Input() empKinInfo: EmployeeKinInfo;
  rForm: FormGroup;
  response: ResponseModel = {};
  @ViewChild('btnKinInfoCancel') cancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  relationShipList: RelationShip[];
  getErrorMessage: any;
  isShownrelation: boolean = false ;
  list: RelationShip[];
  selectedType: any;
  selectedName: string;
  constructor(private route: ActivatedRoute,private commonService: CommonService, 
    private fb: FormBuilder, private empService: EmpDetailService, private notificationService: NotificationService) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.empKinInfo.firstName !== undefined) {
      this.route.paramMap.subscribe((params: any) => {
        this.empKinInfo.employeeId = Number(params.params.id);
      });
    }
  }

  ngOnInit(): void {
    this.createForm();
    this.getrelationShip();
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
  createForm() {
    this.rForm = this.fb.group({
      firstName: [null, Validators.required],
      middelName: [null, Validators.nullValidator],
      lastName: [null, Validators.required],
      relationShip: [null, Validators.required],
      contactNo: [null,[Validators.maxLength(16),Validators.nullValidator]],
      email:  [null, Validators.compose([Validators.nullValidator, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      otherrelation: [null, Validators.nullValidator],
    });
  }

  editKinDetails() {
    this.rForm.get('firstName').patchValue(this.empKinInfo.firstName);
    this.rForm.get('middelName').patchValue(this.empKinInfo.middelName);
    this.rForm.get('lastName').patchValue(this.empKinInfo.lastName);
    this.rForm.get('relationShip').patchValue(this.empKinInfo.relationShip == 0? null :this.empKinInfo.relationShip);
    this.rForm.get('contactNo').patchValue(this.empKinInfo.contactNo);
    this.rForm.get('email').patchValue(this.empKinInfo.email);
    if(this.empKinInfo.otherRelation!=null && this.empKinInfo.otherRelation!=""){
      this.rForm.get('otherrelation').patchValue(this.empKinInfo.otherRelation);
      this.isShownrelation=true;
    }
    else{
      this.isShownrelation=false;
    }
  }

  get firstName() {
    return this.rForm.get('firstName');
  }

  get middelName() {
    return this.rForm.get('middelName');
  }

  get lastName() {
    return this.rForm.get('lastName');
  }

  get relationShip() {
    return this.rForm.get('relationShip');
  }
  get contactNo() {
    return this.rForm.get('contactNo');
  }
  get email() {
    return this.rForm.get('email');
  }
  get otherrelation() {
    return this.rForm.get('otherrelation');
  }
  UpdateKinInfo() {
    if (this.rForm.valid) {
      const data = {
        EmployeeId: this.empKinInfo.employeeId,
        FirstName: this.rForm.get('firstName').value,
        MiddelName: this.rForm.get('middelName').value,
        LastName: this.rForm.get('lastName').value,
        RelationShip: this.rForm.get('relationShip').value,
        ContactNo: String(this.rForm.get('contactNo').value),
        Email: this.rForm.get('email').value,
        OtherRelation: this.rForm.get('otherrelation').value
      }
      this.empService.updateEmployeeKinInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.empKinInfo = this.response.responseData;
            this.cancel.nativeElement.click();
            this.formDirective.resetForm();
            this.notificationService.Success({ message: 'KinInfo updated successfully', title: null });
            break;

          default:
            break;
        }
      });
    }
  }

  cancelModal() {
    this.rForm.reset();
    this.formDirective.resetForm();
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

  }
   }
}

