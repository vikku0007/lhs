import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { EmployeeComplianceDetails } from 'projects/dashboard/src/lib/viewmodel/employee-compliance-details';
import { EmpServiceService } from 'projects/dashboard/src/lib/services/emp-service.service';

@Component({
  selector: 'lib-employee-documentchecklist',
  templateUrl: './employee-documentchecklist.component.html',
  styleUrls: ['./employee-documentchecklist.component.scss']
})

export class EmployeeDocumentchecklistComponent {
  dataSource :any;
  displayedColumns = ['name','codedata', 'descriptions'];
  documentdata=[];
  spans = [];
  responseModel: ResponseModel = {};
  tempRowId = null;
  tempRowCount = null;
  ClientId: number;
  CheckListdata =[];
  listcount:any;
  doocumentcheckList: EmployeeComplianceDetails[];
  requiredComplianceModel: EmployeeComplianceDetails[] = [];
  acheivesArray: { description?: string }[] = [];
  paging: Paging = {};
  datasourcelistdata = [];
  EmployeeId: number;
  constructor(private route: ActivatedRoute, private empService: EmpServiceService, private fb: FormBuilder,
    private notificationService: NotificationService, private commonService: CommonService,private router:Router) {
    }

  ngOnInit(): void {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.route.queryParams.subscribe(params => {
    this.EmployeeId = parseInt(params.Id);
    });
   this.getDocumentName();
   this.getDocumentDetails();
  }
  getDocumentName(){
    this.commonService.GetEmpCheckListDocument().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.documentdata=this.responseModel.responseData||[];
        this.cacheSpan('Priority', d => d.value);
        this.cacheSpan('Name', d => d.codeDescription);
      }else{
    }
    }));
  }

  getDocumentDetails() {
    const data = {
      EmployeeId: this.EmployeeId,
      PageSize: this.paging.pageSize,
      pageNo: this.paging.pageNo,
    }
    debugger
    this.empService.getRequireComplianceList(data).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.requiredComplianceModel = this.responseModel.responseData;
          this.CheckListdata = this.requiredComplianceModel;
          const checkarray = [];
          this.acheivesArray = [];
          this.CheckListdata.forEach(element => {
          const studentdata = {
           documentTypeName: element.document,
          id:element.id
          };
          checkarray.push(studentdata);
          this.doocumentcheckList = checkarray;
          });
          this.datasourcelistdata=this.dataSource.filteredData;
         
          for (let i = 0; i < this.datasourcelistdata.length; i++) {
          if (this.doocumentcheckList.findIndex(x => x.documentTypeName === this.datasourcelistdata[i].codeDescription) > -1) {
          (document.getElementById(this.removeSpace(this.datasourcelistdata[i].codeDescription)+ 'check') as HTMLInputElement).checked = true;
          (document.getElementById(this.removeSpace(this.datasourcelistdata[i].codeDescription)+ 'check') as HTMLInputElement).disabled = true;
          this.acheivesArray.push({description:this.datasourcelistdata[i].codeDescription});
          }
          else {
          (document.getElementById(this.removeSpace(this.datasourcelistdata[i].codeDescription)+ 'check') as HTMLInputElement).checked = false;
        }
      }
    }
    });
  }
  removeSpace(value) {
  let a = value.replace(/\s/g, '');
    return a.toLowerCase();
  }
  cacheSpan(key, accessor) {
    debugger
    for (let i = 0; i < this.documentdata.length;) {
      let currentValue = accessor(this.documentdata[i]);
      let count = 1;
    for (let j = i + 1; j < this.documentdata.length; j++) {
      if (currentValue != accessor(this.documentdata[j])) {
      break;
    }
       count++;
      }
    if (!this.spans[i]) {
        this.spans[i] = {};
      }
      // Store the number of similar values that were found (the span)
      // and skip i to the next unique row.
      this.spans[i][key] = count;
      i += count;
    }
    this.dataSource = new MatTableDataSource(this.documentdata);
  }

  getRowSpan(col, index) {
    return this.spans[index] && this.spans[index][col];
  }
  ClientDocument(){
    this.router.navigate(['/client/client-document-checklist'], { queryParams: { Id:this.ClientId } });
  }
}
export interface PeriodicElement {
  sectionTitle: string;
  name: string;
  descriptions: string[];
}
