import { Routes, RouterModule } from '@angular/router';

import { NgModule } from '@angular/core';
import { CalendarComponent } from './components/calendar/calendar.component';

const routes: Routes = [
    { path: '', component: CalendarComponent },
    { path: ':id', component: CalendarComponent }
];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ClientRosterRoutingModule { }