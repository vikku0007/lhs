import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/lib/services/api-service/api.service';
import { map } from 'rxjs/operators';
import { Constants } from '../config/constants';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private apiService: ApiService) {

  }

  login(data) {
    return this.apiService.post(Constants.login, data, null).pipe(map(res => {
      return res;
    }))
    
  }
  forgotPassword(data) {
    return this.apiService.post(Constants.forgotPassword, data, null).pipe(map(res => {
      return res;
    }))
    
  }
  emailConfirmation(data) {
    return this.apiService.post(Constants.emailConfirmation, data, null).pipe(map(res => {
      return res;
    }))
    
  }
  resetPassword(data) {
    return this.apiService.post(Constants.resetPassword, data, null).pipe(map(res => {
      return res;
    }))
    
  }
}
