import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ShiftDetailComponent } from './components/shift-detail/shift-detail.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { ShiftListComponent } from './components/shift-list/shift-list.component';
import { LeaveComponent } from './components/leave/leave.component';
import { LeaveApprovedComponent } from './components/leave-approved/leave-approved.component';


const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: ':id', component: DashboardComponent },
  { path: 'view-shift', component: ShiftDetailComponent },
  { path: 'view-shift/:id', component: ShiftDetailComponent },
  { path: 'checkout', component: CheckoutComponent },
  { path: 'checkout/:id', component: CheckoutComponent },
  { path: 'shift-list', component: ShiftListComponent },
  { path: 'shift-list/:id', component: ShiftListComponent },
  { path: 'leave/:id', component: LeaveComponent },
  { path: 'leaveapprove/:id', component: LeaveApprovedComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeDashboardRoutingModule { }
