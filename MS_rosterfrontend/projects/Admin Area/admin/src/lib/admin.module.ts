import { NgModule } from '@angular/core';
import { AdminComponent } from './admin.component';
import { AdminRoutingModule  } from './admin-routing.module';
import { LocationComponent } from './components/Administartion/location/location.component';
import { EditLocationComponent } from './components/Administartion/edit-location/edit-location.component';
import { MatTableModule } from '@angular/material/table';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatOptionModule, MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { CommonModule } from '@angular/common';
import { MasterEntriesComponent } from './Components/Administartion/master-entries/master-entries.component';
import { PublicHolidayComponent } from './Components/Administartion/public-holiday/public-holiday.component';
import { AuditLogComponent } from './Components/Administartion/audit-log/audit-log.component';
import { ToDoItemComponent } from './Components/Administartion/to-do-item/to-do-item.component';
import { GlobalPayrateComponent } from './Components/Administartion/global-payrate/global-payrate.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { UploadServicePriceComponent } from './Components/Administartion/upload-service-price/upload-service-price.component';
import { AddLocationComponent } from './Components/Administartion/add-location/add-location.component';

@NgModule({
  declarations: [AdminComponent,LocationComponent,EditLocationComponent, MasterEntriesComponent, PublicHolidayComponent, AuditLogComponent, ToDoItemComponent, GlobalPayrateComponent, UploadServicePriceComponent, AddLocationComponent],
  imports: [CommonModule,AdminRoutingModule, MatTableModule, LhsComponentModule, FormsModule, ReactiveFormsModule, MatPaginatorModule,
    MatFormFieldModule, MatSortModule, MatInputModule, MatSelectModule, MatRadioModule, MatNativeDateModule, MatOptionModule,
    MatDatepickerModule,MatCheckboxModule,NgxMatSelectSearchModule
  ],
  exports: [AdminComponent]
})
export class AdminModule { }
