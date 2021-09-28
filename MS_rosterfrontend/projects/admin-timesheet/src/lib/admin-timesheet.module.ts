import { NgModule } from '@angular/core';
import { AdminTimesheetComponent } from './admin-timesheet.component';
import { TimesheetComponent } from './components/timesheet/timesheet.component';
import { AdminTimesheetRoutingModule } from './admin-timesheet-routing.module';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { DayPilotModule } from 'daypilot-pro-angular';
import { HourlyTimesheetComponent } from './components/hourly-timesheet/hourly-timesheet.component';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatRadioModule } from '@angular/material/radio';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CommonModule } from '@angular/common';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';



@NgModule({
  declarations: [AdminTimesheetComponent, TimesheetComponent, HourlyTimesheetComponent],
  imports: [AdminTimesheetRoutingModule,NgxMatSelectSearchModule, LhsComponentModule, DayPilotModule, CommonModule,
    MatFormFieldModule, MatDatepickerModule, MatSortModule, MatInputModule, MatSelectModule, MatRadioModule, MatOptionModule,
    MatNativeDateModule, DayPilotModule, FormsModule, ReactiveFormsModule, DayPilotModule, MatProgressSpinnerModule, MatCheckboxModule
  ],
  exports: [AdminTimesheetComponent]
})
export class AdminTimesheetModule { }
