import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from 'projects/auth/src/lib/components/login/login.component';
import { AuthModule } from 'projects/auth/src/public-api';
import { DashboardModule } from 'projects/dashboard/src/projects';
import { AdminModule } from 'projects/Admin Area/admin/src/projects';
import { AuthAdmin } from 'projects/core/src/lib/services/auth-service/auth.guard';
import { ClientModule } from 'projects/client/src/projects';
import { RosterModule } from 'projects/roster/src/projects';
import { EmployeePayrollModule } from 'projects/employee-payroll/src/projects';
import { AdminDashboardModule } from 'projects/admin-dashboard/src/projects';
import { EmployeeDashboardModule } from 'projects/employee-dashboard/src/projects';
import { EmployeeOthersModule } from 'projects/employee-others/src/projects';
import { EmployeeCheckoutModule } from 'projects/employee-checkout/src/projects';
import { EmployeeRosterModule } from 'projects/employee-roster/src/projects';
import { ClientDashboardModule } from 'projects/client-dashboard/src/projects';
import { ClientRosterModule } from 'projects/client-roster/src/projects';
import { EmployeeDetailModule } from 'projects/employee-detail/src/project';
import { AdminTimesheetModule } from 'projects/admin-timesheet/src/projects';


const routes: Routes = [
  {
    path: '',
    component: LoginComponent
  },
  {
    path: 'auth',
    loadChildren: () => AuthModule
  },
  {
    path: 'employee',
    loadChildren: () => DashboardModule,
    canActivate: [AuthAdmin]
  },
  {
    path: 'admin',
    loadChildren: () => AdminModule,
    canActivate: [AuthAdmin]
  },
  {
    path: 'client',
    loadChildren: () => ClientModule,
    canActivate: [AuthAdmin]
  },
  {
    path: 'roster',
    loadChildren: () => RosterModule,
    canActivate: [AuthAdmin]
  },
  {
    path: 'admin/dashboard',
    loadChildren: () => AdminDashboardModule,
    canActivate: [AuthAdmin]
  },  
  {
    path: 'admin/employee-payroll',
    loadChildren: () => EmployeePayrollModule,
    canActivate: [AuthAdmin]
  },
  {
    path: 'admin/timesheet',
    loadChildren: () => AdminTimesheetModule,
    canActivate: [AuthAdmin]
  },
  {
    path: 'employee/dashboard',
    loadChildren: () => EmployeeDashboardModule
  },
  {
    path: 'employee/others',
    loadChildren: () => EmployeeOthersModule
  },
  {
    path: 'employee/logout',
    loadChildren: () => EmployeeCheckoutModule
  },
  {
    path: 'employee/roster',
    loadChildren: () => EmployeeRosterModule
  },
  {
    path: 'client/dashboard',
    loadChildren: () => ClientDashboardModule
  },
  {
    path: 'client/roster',
    loadChildren: () => ClientRosterModule
  },
  {
    path: 'employeedetail',
    loadChildren: () => EmployeeDetailModule
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
