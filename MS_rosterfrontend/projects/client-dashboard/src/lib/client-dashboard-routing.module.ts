import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NgModule } from '@angular/core';
import { ViewShiftComponent } from './components/view-shift/view-shift.component';

const routes: Routes = [
    { path: '', component: DashboardComponent },    
    { path: ':id', component: DashboardComponent },   
    { path: 'view-shift/:id', component: ViewShiftComponent }
];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ClientDashboardRoutingModule { }