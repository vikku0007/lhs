import { NgModule } from '@angular/core';
import { EmployeePayrollComponent } from './employee-payroll.component';
import { EmployeePayrollRoutingModule } from './employee-payroll-routing.module';
import { EmployeePayrollListComponent } from './components/employee-payroll-list/employee-payroll-list.component';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule, DatePipe } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { MatNativeDateModule, MatOptionModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { LhsServiceModule } from 'projects/lhs-service/src/lib/lhs-service.module';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { EmployeeRejectedshiftPayrollListComponent } from './components/employee-rejectedshift-payroll-list/employee-rejectedshift-payroll-list.component';
import { MatTableExporterModule } from 'mat-table-exporter';
import { EmployeeIncompletePayrollComponent } from './components/employee-incomplete-payroll/employee-incomplete-payroll.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { PayrollInMyobComponent } from './components/payroll-in-myob/payroll-in-myob.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';

@NgModule({
  declarations: [EmployeePayrollComponent, EmployeePayrollListComponent, EmployeeRejectedshiftPayrollListComponent, EmployeeIncompletePayrollComponent, PayrollInMyobComponent],
  imports: [EmployeePayrollRoutingModule, MatFormFieldModule, MatSortModule, MatInputModule, MatSelectModule, MatOptionModule,
    CommonModule, LhsServiceModule, MatTableModule, FormsModule, LhsComponentModule, MatNativeDateModule,
    ReactiveFormsModule, MatDatepickerModule, MatTableExporterModule, MatPaginatorModule, TimepickerModule.forRoot(),NgxMatSelectSearchModule
  ],
  exports: [EmployeePayrollComponent],
  providers: [DatePipe, { provide: MAT_DATE_LOCALE, useValue: 'en-GB' }
] 
})
export class EmployeePayrollModule { }
