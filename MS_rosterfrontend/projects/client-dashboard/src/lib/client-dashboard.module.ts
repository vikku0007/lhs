import { NgModule } from '@angular/core';
import { ClientDashboardComponent } from './client-dashboard.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ClientDashboardRoutingModule } from './client-dashboard-routing.module';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { CurrentShiftComponent } from './components/current-shift/current-shift.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { LhsServiceModule } from 'projects/lhs-service/src/projects';
import { MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { AssignedShiftComponent } from './components/assigned-shift/assigned-shift.component';
import { ViewShiftComponent } from './components/view-shift/view-shift.component';


@NgModule({
  declarations: [ClientDashboardComponent, DashboardComponent, CurrentShiftComponent, AssignedShiftComponent, ViewShiftComponent],
  imports: [ClientDashboardRoutingModule, LhsComponentModule, MatFormFieldModule, MatSortModule, MatInputModule, MatSelectModule,
    MatRadioModule, MatNativeDateModule, MatOptionModule, MatDatepickerModule, CommonModule, MatCheckboxModule, LhsServiceModule,
    MatTableModule, FormsModule, ReactiveFormsModule, MatPaginatorModule,
  ],
  exports: [ClientDashboardComponent]
})
export class ClientDashboardModule { }
