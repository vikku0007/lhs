import { NgModule } from '@angular/core';
import { EmployeeDashboardComponent } from './employee-dashboard.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { EmployeeDashboardRoutingModule } from './employee-dashboard-routing.module';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { LhsServiceModule } from 'projects/lhs-service/src/lib/lhs-service.module';
import { MatTableModule } from '@angular/material/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatNativeDateModule, MatOptionModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginatorModule } from '@angular/material/paginator';
import { AssignedshiftComponent } from './components/assignedshift/assignedshift.component';
import { CurrentshiftComponent } from './components/currentshift/currentshift.component';
import { ShiftDetailComponent } from './components/shift-detail/shift-detail.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { ShiftListComponent } from './components/shift-list/shift-list.component';
import { LeaveComponent } from './components/leave/leave.component';
import { LeaveApprovedComponent } from './components/leave-approved/leave-approved.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';



@NgModule({
  declarations: [EmployeeDashboardComponent, DashboardComponent, AssignedshiftComponent, CurrentshiftComponent, ShiftDetailComponent, CheckoutComponent, ShiftListComponent, LeaveComponent, LeaveApprovedComponent],
  imports: [ EmployeeDashboardRoutingModule, LhsComponentModule, MatTableModule, FormsModule, ReactiveFormsModule, MatPaginatorModule,
    MatFormFieldModule, MatSortModule, MatInputModule, MatSelectModule, MatRadioModule, MatNativeDateModule, MatOptionModule,
    MatDatepickerModule, CommonModule, MatCheckboxModule, LhsServiceModule, NgxMatSelectSearchModule
  ],
  exports: [EmployeeDashboardComponent],
  providers:[{ provide: MAT_DATE_LOCALE, useValue: 'en-GB' }]
})
export class EmployeeDashboardModule { }
