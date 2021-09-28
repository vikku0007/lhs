import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { ClientComplianceDetails } from '../../view-models/Client-compliance-details';
import { ClientService } from '../../services/client.service';

@Component({
  selector: 'lib-client-document-checklist',
  templateUrl: './client-document-checklist.component.html',
  styleUrls: ['./client-document-checklist.component.scss']
})

export class ClientDocumentChecklistComponent {
  dataSource :any;
  displayedColumns = ['sectionTitle', 'name','mandatoryopt', 'descriptions'];
  documentdata=[];
  spans = [];
  responseModel: ResponseModel = {};
  tempRowId = null;
  tempRowCount = null;
  ClientId: number;
  CheckListdata =[];
  listcount:any;
  doocumentcheckList: ClientComplianceDetails[];
  requiredComplianceModel: ClientComplianceDetails[] = [];
  acheivesArray: { description?: string }[] = [];
  paging: Paging = {};
  datasourcelistdata = [];
  constructor(private route: ActivatedRoute, private clientService: ClientService, private fb: FormBuilder,
    private notificationService: NotificationService, private commonService: CommonService,private router:Router) { 
    }

  ngOnInit(): void {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.route.queryParams.subscribe(params => {
    this.ClientId = parseInt(params.Id);
    });
   this.getDocumentName();
   this.getDocumentDetails();
  }
  getDocumentName(){
    this.commonService.GetCheckListDocument().subscribe((res=>{
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
      ClientId: this.ClientId,
      PageSize: this.paging.pageSize,
      pageNo: this.paging.pageNo,
    }
    this.clientService.GetClientCompliancesList(data).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.requiredComplianceModel = this.responseModel.responseData;
          this.CheckListdata = this.requiredComplianceModel;
          const checkarray = [];
          this.acheivesArray = [];
          this.CheckListdata.forEach(element => {
          const studentdata = {
          document: element.document,
          id:element.id
          };
          checkarray.push(studentdata);
          this.doocumentcheckList = checkarray;
          });
          this.datasourcelistdata=this.dataSource.filteredData;
          debugger;
          for (let i = 0; i < this.datasourcelistdata.length; i++) {
          if (this.doocumentcheckList.findIndex(x => x.document === this.datasourcelistdata[i].codeDescription) > -1) {
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
const originalData = [
  { sectionTitle:'Support planning', name:  '', descriptions: ['Client Profile and Support Information', 'Day to day routine', 'Entry and exit checklist', 'Application form', 'NDIS plan', 'Risk assessment/management', 'Service Agreement', 'Residential/SDA Agreement (if Applicable)', 'Medication Treatment sheets', 'Health notes (relating to specific health concerns, doctors’ appointments, etc..)'] },
  { sectionTitle:'Behavior support/specialist reports', name: '', descriptions: ['Behavioral Assessment Report', 'Managing incidents behavior safety plan (behavior support plan)', 'Speech pathologist report (including visuals cues) –if applicable', 'OT report', 'Psychologists Initial Report (NDIS)', 'Community Access guidelines'] },
  { sectionTitle:'Individualized documents', name: '', descriptions: ['daily progress notes ', 'Day Report Book', 'Asset register', 'Weight monitor –if applicable', 'Weekly menu/ shopping list'] },
  { sectionTitle:'Health planning', name: '', descriptions: ['Health Support Summary Needs- if applicable', 'Specific Health Management plans – if applicable'] },
  { sectionTitle:'Health notes/ Hospital admission documents', name: 'Boron', descriptions: ['Hospital admissions forms'] },
  { sectionTitle:'Section 7 CHAPS', name: 'Carbon', descriptions: ['Comprehensive Health Assessments (CHAPS)- for long-term respite residents. N/A'] },
  { sectionTitle:'Treatment sheets/doctors forms', name: 'Nitrogen', descriptions: ['Medication administration blank sign sheets (for future dates- includes webster pack and original container)'] },
  { sectionTitle:'Incident reporting/ Complaint/ Feedback ', name: 'Oxygen', descriptions: ['Incident reports', 'Complaint Form', 'Feedback Form'] },
]
const DATA = originalData.reduce((current, next) => {
  next.descriptions.forEach(b => {
    current.push({ sectionTitle: next.sectionTitle, name: b, descriptions: b })
  });
  return current;
}, []);
console.log(DATA)

const ELEMENT_DATA: PeriodicElement[] = DATA;