import { NgModule } from '@angular/core';
import { CoreComponent } from './core.component';
import { CookieModule, CookieService } from 'ngx-cookie';
import { ErrorHandlerService } from './services/error-handler/error-handler.service';
import { ApiService } from './services/api-service/api.service';
import { MembershipService } from './services/membership-service/membership.service';
import { InterceptorService } from './services/interceptor/interceptor.service';
import { AuthService } from 'projects/auth/src/public-api';
import { ToastrModule } from 'ngx-toastr';


@NgModule({
  declarations: [CoreComponent],
  imports: [CookieModule.forRoot(), ToastrModule.forRoot()
  ],
  providers: [CookieService, ErrorHandlerService, ApiService, MembershipService, InterceptorService],
  exports: [CoreComponent]
})
export class CoreModule { }
