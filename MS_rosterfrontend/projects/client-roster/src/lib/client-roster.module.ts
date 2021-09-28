import { NgModule } from '@angular/core';
import { ClientRosterComponent } from './client-roster.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { ClientRosterRoutingModule } from './client-roster-routing.module';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { DayPilotModule } from 'daypilot-pro-angular';



@NgModule({
  declarations: [ClientRosterComponent, CalendarComponent],
  imports: [ClientRosterRoutingModule, LhsComponentModule, DayPilotModule
  ],
  exports: [ClientRosterComponent]
})
export class ClientRosterModule { }
