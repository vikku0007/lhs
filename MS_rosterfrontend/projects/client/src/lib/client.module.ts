import { NgModule } from '@angular/core';
import { ClientComponent } from './client.component';
import { ClientRoutingModule } from './client-routing.module';
import { LhsComponentModule, SidebarComponent, HeaderComponent } from 'projects/lhs-component/src/projects';
import { LhsServiceModule } from 'projects/lhs-service/src/projects';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatOptionModule, MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTableModule } from '@angular/material/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { ClientListComponent } from './components/client-list/client-list.component';
import { ClientDetailsComponent } from './components/client-details/client-details.component';
import { AccidentIncidentDetailsComponent } from './components/accident-incident-details/accident-incident-details.component';
import { PageSubmenuComponent } from './components/page-submenu/page-submenu.component';
import { UploadImageComponent } from './components/upload-image/upload-image.component';
import { UserCardComponent } from './components/user-card/user-card.component';
import { AccidentIncidentListComponent } from './components/accident-incident-list/accident-incident-list.component';
import { DocumentDetailsComponent } from './components/document-details/document-details.component';
import { DocumentListComponent } from './components/document-list/document-list.component';
import { MedicalHistoryDetailsComponent } from './components/medical-history-details/medical-history-details.component';
import { MedicalHistoryListComponent } from './components/medical-history-list/medical-history-list.component';
import { ProgressNotesDetailsComponent } from './components/progress-notes-details/progress-notes-details.component';
import { ProgressNotesListComponent } from './components/progress-notes-list/progress-notes-list.component';
import { ClientPrimaryCareerInfoComponent } from './components/client-primarycareerinfo/client-primarycareerinfo.component';
import { ClientOnboadingnotesComponent } from './components/client-onboadingnotes/client-onboadingnotes.component';
import { ClientAdditionalNotesComponent } from './components/client-additional-notes/client-additional-notes.component';
import { ClientFundinginfoComponent } from './components/client-fundinginfo/client-fundinginfo.component';
import { ClientBasicInfoComponent } from './components/client-basic-info/client-basic-info.component';
import { LhsDirectivesModule } from 'projects/lhs-directives/src/projects';
import { ClientGuardianiInformationComponent } from './components/client-guardiani-information/client-guardiani-information.component';
import { ClientProfileInfoComponent } from './components/client-profile-info/client-profile-info.component';
import { ClientDocumentChecklistComponent } from './components/client-document-checklist/client-document-checklist.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { SupportCoordinatorInfoComponent } from './components/support-coordinator-info/support-coordinator-info.component';
import {NgxPrintModule} from 'ngx-print';
import { AgmCoreModule } from '@agm/core';
import { ClientShiftListComponent } from './components/client-shift-list/client-shift-list.component';
import { MedicalShiftListComponent } from './components/medical-shift-list/medical-shift-list.component';
import { ProgressShiftListComponent } from './components/progress-shift-list/progress-shift-list.component';


//import { PhoneNumberDirective } from 'projects/lhs-directive/src/directives/phone-number.directive';

//import { ClientPrimarycareerinfoComponent } from './components/client-primarycareerinfo/client-primarycareerinfo.component';


@NgModule({
  declarations: [ClientComponent, ClientListComponent, ClientDetailsComponent, AccidentIncidentDetailsComponent, PageSubmenuComponent, UploadImageComponent, UserCardComponent, AccidentIncidentListComponent, DocumentDetailsComponent, DocumentListComponent,
     MedicalHistoryDetailsComponent, MedicalHistoryListComponent, ProgressNotesDetailsComponent, 
     ProgressNotesListComponent,ClientPrimaryCareerInfoComponent, ClientOnboadingnotesComponent,
      ClientAdditionalNotesComponent, ClientFundinginfoComponent, ClientBasicInfoComponent, ClientGuardianiInformationComponent, ClientProfileInfoComponent, ClientDocumentChecklistComponent, SupportCoordinatorInfoComponent, ClientShiftListComponent, MedicalShiftListComponent, ProgressShiftListComponent
      
    ],
  imports: [ClientRoutingModule, LhsComponentModule, MatTableModule, FormsModule, ReactiveFormsModule, MatPaginatorModule,
    MatFormFieldModule, MatSortModule, MatInputModule, MatSelectModule, MatRadioModule, MatNativeDateModule, MatOptionModule,
    MatDatepickerModule, CommonModule, MatCheckboxModule, LhsServiceModule,LhsDirectivesModule,
   NgxPrintModule, NgxMatSelectSearchModule
    
  ],
  exports: [ClientComponent]
})
export class ClientModule { }
