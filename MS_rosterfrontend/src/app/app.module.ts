import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CoreModule, InterceptorService } from 'projects/core/src/projects';
import { CORE_CONFIG } from 'projects/core/src/lib/config/core-config';
import { CoreConfigFactory } from './domain/services/core-config.factory';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthAdmin } from 'projects/core/src/lib/services/auth-service/auth.guard';
import { LhsDirectivesModule } from 'projects/lhs-directives/src/projects';
import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';

@NgModule({
  declarations: [
    AppComponent

  ],
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    CoreModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatProgressSpinnerModule,
    LhsDirectivesModule,
    NgIdleKeepaliveModule.forRoot(),
  ],
  providers: [{ provide: CORE_CONFIG, useFactory: CoreConfigFactory },
  { provide: HTTP_INTERCEPTORS, useClass: InterceptorService, multi: true }, AuthAdmin],
  bootstrap: [AppComponent],
  exports: [LhsDirectivesModule]
})
export class AppModule { }
