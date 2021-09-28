import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NgModule } from '@angular/core';
import { AdminNotificationComponent } from './components/admin-notification/admin-notification.component';

const routes: Routes = [
    { path: '', component: DashboardComponent },
    { path: 'notification', component: AdminNotificationComponent }
];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminDashboardRoutingModule { }