import { NgModule } from '@angular/core';
import { EmployeeRosterComponent } from './employee-roster.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { EmployeeRosterRoutingModule } from './employee-roster-routing.module';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { DayPilotModule } from 'daypilot-pro-angular';

@NgModule({
  declarations: [EmployeeRosterComponent, CalendarComponent],
  imports: [EmployeeRosterRoutingModule, LhsComponentModule, DayPilotModule
  ],
  exports: [EmployeeRosterComponent]
})
export class EmployeeRosterModule { }
