import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { TimesheetComponent } from './components/timesheet/timesheet.component';
import { HourlyTimesheetComponent } from './components/hourly-timesheet/hourly-timesheet.component';

const routes: Routes = [
    { path: '', component: TimesheetComponent },
    { path: 'hourly-timesheet', component: HourlyTimesheetComponent }
];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminTimesheetRoutingModule { }