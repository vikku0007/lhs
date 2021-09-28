import { NgModule } from '@angular/core';
import { AdminDashboardComponent } from './admin-dashboard.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { AdminDashboardRoutingModule } from './admin-dashboard-routing.module';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSortModule } from '@angular/material/sort';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LhsServiceModule } from 'projects/lhs-service/src/projects';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatPaginatorModule } from '@angular/material/paginator';
import { ClientFundingListComponent } from './components/client-funding-list/client-funding-list.component';
import { ChartsModule } from 'ng2-charts';
import { AdminNotificationComponent } from './components/admin-notification/admin-notification.component';


@NgModule({
  declarations: [AdminDashboardComponent, DashboardComponent, ClientFundingListComponent, AdminNotificationComponent],
  imports: [LhsComponentModule, AdminDashboardRoutingModule, MatTableModule, FormsModule, ReactiveFormsModule, MatPaginatorModule,
    MatFormFieldModule, MatSortModule, MatInputModule, MatSelectModule, MatRadioModule, MatNativeDateModule, MatOptionModule,
    MatDatepickerModule, CommonModule, MatCheckboxModule, LhsServiceModule,ChartsModule
  ],
  exports: [AdminDashboardComponent]
})
export class AdminDashboardModule { }
