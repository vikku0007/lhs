import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { RosterComponent } from './roster.component';
import { SchedulerComponent } from './components/scheduler/scheduler.component';
import { AddShiftComponent } from './components/add-shift/add-shift.component';
import { ShiftListComponent } from './components/shift-list/shift-list.component';
import { ViewShiftComponent } from './components/view-shift/view-shift.component';
import { EditShiftComponent } from './components/edit-shift/edit-shift.component';
import { LocationViewComponent } from './components/location-view/location-view.component';

const routes: Routes = [
    { path: '', component: SchedulerComponent },
    { path: 'scheduler', component: SchedulerComponent },    
    { path: 'add-shift', component: AddShiftComponent },
    { path: 'view-shift', component: ViewShiftComponent },
    { path: 'view-shift/:Id', component: ViewShiftComponent },
    { path: 'edit-shift', component: EditShiftComponent },
    { path: 'edit-shift/:Id', component: EditShiftComponent },
    { path: 'shifts', component: ShiftListComponent },
    { path: 'location-view', component: LocationViewComponent }
];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class RosterRoutingModule { }