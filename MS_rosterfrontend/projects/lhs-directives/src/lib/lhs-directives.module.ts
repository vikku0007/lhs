import { NgModule } from '@angular/core';
import { LhsDirectivesComponent } from './lhs-directives.component';
import { PhoneNumberDirective } from './directives/phone-number.directive';
//import { AppDateAdapter } from './directives/date-format.directive';



@NgModule({
  declarations: [LhsDirectivesComponent,PhoneNumberDirective],
  imports: [
  ],
  exports: [LhsDirectivesComponent,PhoneNumberDirective]
})
export class LhsDirectivesModule { }
