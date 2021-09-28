import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmployeeIncompletePayrollComponent } from './components/employee-incomplete-payroll/employee-incomplete-payroll.component';
import { EmployeePayrollListComponent } from './components/employee-payroll-list/employee-payroll-list.component';
import { EmployeeRejectedshiftPayrollListComponent } from './components/employee-rejectedshift-payroll-list/employee-rejectedshift-payroll-list.component';
import { PayrollInMyobComponent } from './components/payroll-in-myob/payroll-in-myob.component';


const routes: Routes = [
  { path: '', component: EmployeePayrollListComponent },
  { path: 'employee-payroll', component: EmployeePayrollListComponent },
  { path: 'employee-rejected-payroll', component: EmployeeRejectedshiftPayrollListComponent },
  { path: 'employee-incomplete-payroll', component: EmployeeIncompletePayrollComponent },
  { path: 'payroll-in-myob', component: PayrollInMyobComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeePayrollRoutingModule { }
