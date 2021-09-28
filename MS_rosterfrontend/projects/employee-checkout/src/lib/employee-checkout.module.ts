import { NgModule } from '@angular/core';
import { EmployeeCheckoutComponent } from './employee-checkout.component';
import { LogoutComponent } from './components/logout/logout.component';
import { EmployeeCheckoutRoutingModule } from './employee-checkout-routing.module';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSortModule } from '@angular/material/sort';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { LhsServiceModule } from 'projects/lhs-service/src/lib/lhs-service.module';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatRadioModule } from '@angular/material/radio';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { LhsDirectivesModule } from 'projects/lhs-directives/src/projects';
import { MatPaginatorModule } from '@angular/material/paginator';
import { AccidentIncidentComponent } from './components/accident-incident/accident-incident.component';
import { MedicalHistoryComponent } from './components/medical-history/medical-history.component';
import { ProgressNotesComponent } from './components/progress-notes/progress-notes.component';
import { ClientDocumentsComponent } from './components/client-documents/client-documents.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { IncidentProviderdetailComponent } from './components/incident-providerdetail/incident-providerdetail.component';
import { IncidentContactpersonComponent } from './components/incident-contactperson/incident-contactperson.component';
import { IncidentCategoryComponent } from './components/incident-category/incident-category.component';
import { IncidentDetailsComponent } from './components/incident-details/incident-details.component';
import { IncidentImpactedpersonComponent } from './components/incident-impactedperson/incident-impactedperson.component';
import { IncidentSubjectallegationComponent } from './components/incident-subjectallegation/incident-subjectallegation.component';
import { IncidentImmediateactionComponent } from './components/incident-immediateaction/incident-immediateaction.component';
import { IncidentRiskassesmentComponent } from './components/incident-riskassesment/incident-riskassesment.component';
import { IncidentAttachmentComponent } from './components/incident-attachment/incident-attachment.component';
import { IncidentDeclarationComponent } from './components/incident-declaration/incident-declaration.component';
import {NgxPrintModule} from 'ngx-print';
import { ToDoListComponent } from './components/to-do-list/to-do-list.component';
import { EditTodoComponent } from './components/edit-todo/edit-todo.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';


@NgModule({
  declarations: [EmployeeCheckoutComponent, LogoutComponent, AccidentIncidentComponent, MedicalHistoryComponent, ProgressNotesComponent, ClientDocumentsComponent, CheckoutComponent, IncidentProviderdetailComponent, IncidentContactpersonComponent, IncidentCategoryComponent, IncidentDetailsComponent, IncidentImpactedpersonComponent, IncidentSubjectallegationComponent, IncidentImmediateactionComponent, IncidentRiskassesmentComponent, IncidentAttachmentComponent, IncidentDeclarationComponent, ToDoListComponent, EditTodoComponent],
  imports: [EmployeeCheckoutRoutingModule, MatTableModule, LhsComponentModule, FormsModule, ReactiveFormsModule, MatPaginatorModule,
    MatFormFieldModule, MatSortModule, MatInputModule, MatSelectModule, MatRadioModule, MatNativeDateModule, MatOptionModule,
    MatDatepickerModule, CommonModule, MatCheckboxModule, LhsServiceModule, LhsDirectivesModule,NgxPrintModule , NgxMatSelectSearchModule
  
  ],
  exports: [EmployeeCheckoutComponent]
})
export class EmployeeCheckoutModule { }
