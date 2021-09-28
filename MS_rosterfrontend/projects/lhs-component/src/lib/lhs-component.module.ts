import { NgModule } from '@angular/core';
import { LhsComponentComponent } from './lhs-component.component';
import { HeaderComponent } from './components/header/header.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { ValidationComponent } from './components/validation/validation.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatOptionModule, MatNativeDateModule } from '@angular/material/core';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { GoogleaddressComponent } from './components/googleaddress/googleaddress.component';
import { GooglePlaceModule } from "ngx-google-places-autocomplete";
import { AdminSidebarComponent } from './components/admin-sidebar/admin-sidebar.component';
import { ClientSidebarComponent } from './components/client-sidebar/client-sidebar.component';

@NgModule({
  declarations: [LhsComponentComponent, HeaderComponent, SidebarComponent, ValidationComponent, GoogleaddressComponent, AdminSidebarComponent, ClientSidebarComponent],
  imports: [MatFormFieldModule,ReactiveFormsModule,FormsModule,MatOptionModule,MatNativeDateModule,
    MatSortModule,MatSelectModule,MatInputModule,MatRadioModule,GooglePlaceModule, CommonModule
  ],
  exports: [LhsComponentComponent, HeaderComponent, SidebarComponent, AdminSidebarComponent, ValidationComponent, GoogleaddressComponent, ClientSidebarComponent]
})
export class LhsComponentModule { }
