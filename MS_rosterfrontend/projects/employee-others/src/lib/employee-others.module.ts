import { NgModule } from '@angular/core';
import { EmployeeOthersComponent } from './employee-others.component';
import { IncidentReportedComponent } from './components/incident-reported/incident-reported.component';
import { EmployeeOthersRoutingModule } from './employee-others-routing.module';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { ComplaintsComponent } from './components/complaints/complaints.component';
import { WarningComponent } from './components/warning/warning.component';
import { DocumentsComponent } from './components/documents/documents.component';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CommonModule } from '@angular/common';
import { MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { LhsServiceModule } from 'projects/lhs-service/src/projects';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatRadioModule } from '@angular/material/radio';
import { LhsDirectivesModule } from 'projects/lhs-directives/src/projects';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatPaginatorModule } from '@angular/material/paginator';
import { CommunicationComponent } from './components/communication/communication.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { IncidentReportedNewComponent } from './components/incident-reported-new/incident-reported-new.component';
import { AccidentIncidentClientDetailsComponent } from './components/accident-incident-client-details/accident-incident-client-details.component';

@NgModule({
  declarations: [EmployeeOthersComponent, IncidentReportedComponent, ComplaintsComponent, WarningComponent, DocumentsComponent, CommunicationComponent, IncidentReportedNewComponent, AccidentIncidentClientDetailsComponent],
  imports: [EmployeeOthersRoutingModule, LhsComponentModule, MatTableModule, FormsModule, ReactiveFormsModule, MatPaginatorModule,
    MatFormFieldModule, MatSortModule, MatInputModule, MatSelectModule, MatRadioModule, MatNativeDateModule, MatOptionModule,
    MatDatepickerModule, CommonModule, MatCheckboxModule, LhsServiceModule, LhsDirectivesModule, NgxMatSelectSearchModule
  ],
  exports: [EmployeeOthersComponent]
})
export class EmployeeOthersModule { }
