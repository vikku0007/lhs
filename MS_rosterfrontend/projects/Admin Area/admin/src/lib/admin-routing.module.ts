import { Routes, RouterModule } from "@angular/router";
import { NgModule } from '@angular/core';
import { AdminComponent } from './admin.component';
import { LocationComponent } from './components/Administartion/location/location.component';
import { EditLocationComponent } from './components/Administartion/edit-location/edit-location.component';
import { MasterEntriesComponent } from './Components/Administartion/master-entries/master-entries.component';
import { PublicHolidayComponent } from './Components/Administartion/public-holiday/public-holiday.component';
import { AuditLogComponent } from './Components/Administartion/audit-log/audit-log.component';
import { ToDoItemComponent } from './Components/Administartion/to-do-item/to-do-item.component';
import { GlobalPayrateComponent } from './Components/Administartion/global-payrate/global-payrate.component';
import { UploadServicePriceComponent } from './Components/Administartion/upload-service-price/upload-service-price.component';
import { AddLocationComponent } from './Components/Administartion/add-location/add-location.component';
const routes: Routes = [
    { path: '', component: AdminComponent },
    { path: 'location', component: LocationComponent },
    { path: 'edit-location', component: EditLocationComponent },
    { path: 'master-entries', component: MasterEntriesComponent },
    { path: 'public-holiday', component: PublicHolidayComponent },
    { path: 'audit-log', component: AuditLogComponent },
    { path: 'to-do-item', component: ToDoItemComponent },
    { path: 'global-payrate', component: GlobalPayrateComponent },
    { path: 'upload-serviceprice', component: UploadServicePriceComponent },
    { path: 'add-location', component: AddLocationComponent }
];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminRoutingModule { }