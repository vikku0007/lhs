import { NgModule } from '@angular/core';
import { RosterComponent } from './roster.component';
import { MatTableModule } from '@angular/material/table';
import { RosterRoutingModule } from './roster-routing.module';
import { SchedulerComponent } from './components/scheduler/scheduler.component';
import { LhsComponentModule } from 'projects/lhs-component/src/projects';
import { AddShiftComponent } from './components/add-shift/add-shift.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatOptionModule, MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CommonModule } from '@angular/common';
import { ShiftListComponent } from './components/shift-list/shift-list.component';
import { LhsServiceModule } from 'projects/lhs-service/src/projects';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MonthlyTimelineComponent } from './components/monthly-timeline/monthly-timeline.component';
import { DayPilotModule } from 'daypilot-pro-angular';
import { DayTimelineComponent } from './components/day-timeline/day-timeline.component';
import { WeekTimelineComponent } from './components/week-timeline/week-timeline.component';
import { ViewShiftComponent } from './components/view-shift/view-shift.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { from } from 'rxjs';
import { RosterSubmenuComponent } from './components/roster-submenu/roster-submenu.component';
import { EditShiftComponent } from './components/edit-shift/edit-shift.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NewdayTimelineComponent } from './components/newday-timeline/newday-timeline.component';
import { LocationViewComponent } from './components/location-view/location-view.component';
import { SidebarCollapsedComponent } from './components/sidebar-collapsed/sidebar-collapsed.component';
import { SidebarContainerComponent } from './components/sidebar-container/sidebar-container.component';
import { SidebarExpandedComponent } from './components/sidebar-expanded/sidebar-expanded.component';
import { SidebarMainComponent } from './components/sidebar-main/sidebar-main.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';

//import {  } from  '@angular/localize/init';

@NgModule({
  declarations: [RosterComponent, SchedulerComponent, AddShiftComponent, ShiftListComponent, MonthlyTimelineComponent,
    DayTimelineComponent, WeekTimelineComponent, ViewShiftComponent, RosterSubmenuComponent, EditShiftComponent, NewdayTimelineComponent, LocationViewComponent, SidebarCollapsedComponent, SidebarContainerComponent, SidebarExpandedComponent, SidebarMainComponent],
  // tslint:disable-next-line: max-line-length
  imports: [RosterRoutingModule, MatTableModule, MatPaginatorModule, LhsComponentModule, CommonModule, LhsServiceModule, MatFormFieldModule, MatDatepickerModule, MatSortModule, MatInputModule, MatSelectModule, MatRadioModule, MatOptionModule, MatNativeDateModule
    , DayPilotModule, FormsModule, ReactiveFormsModule, DayPilotModule, MatProgressSpinnerModule, MatCheckboxModule, NgxMatSelectSearchModule
  ],
  exports: [RosterComponent]
})
export class RosterModule { }
