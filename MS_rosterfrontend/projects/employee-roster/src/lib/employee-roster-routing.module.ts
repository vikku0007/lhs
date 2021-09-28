import { Routes, RouterModule } from '@angular/router';
import { CalendarComponent } from './components/calendar/calendar.component';
import { NgModule } from '@angular/core';

const routes: Routes = [
    { path: 'view', component: CalendarComponent },
    { path: 'view/:id', component: CalendarComponent }
];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EmployeeRosterRoutingModule { }