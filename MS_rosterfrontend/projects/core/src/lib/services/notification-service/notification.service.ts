import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr'

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private toastr: ToastrService) {
    this.toastr.toastrConfig.closeButton = false;
    this.toastr.toastrConfig.autoDismiss = false;
    this.toastr.toastrConfig.tapToDismiss = false;
  }

  Success(body: { message?: string, title?: string }) {
    return this.toastr.success(body.message, body.title, {
      timeOut: 2000
    });
  }

  Error(body: { message?: string, title?: string }) {
    return this.toastr.error(body.message, body.title, {
      timeOut: 2000
    });
  }

  Warning(body: { message?: string, title?: string }) {
    return this.toastr.warning(body.message, body.title, {
      timeOut: 2000
    });
  }
}
