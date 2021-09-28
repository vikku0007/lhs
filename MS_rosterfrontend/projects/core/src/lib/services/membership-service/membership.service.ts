import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { IMembershipModel } from 'projects/viewmodels/i-membership-model';

@Injectable({
  providedIn: 'root'
})
export class MembershipService {
  public loggedIn = new BehaviorSubject<boolean>(false);
  membershipModel: IMembershipModel = {};
  isLoggedIn() {
    return this.loggedIn.asObservable();
  }

  constructor(private cookie: CookieService, private router: Router) { }

  getToken(): string {
    return this.cookie.get('token') ? this.cookie.get('token') : null;
  }

  setCookies(response: IMembershipModel) {
    this.membershipModel = response;
    this.cookie.putObject('userdetails', this.membershipModel);
    this.cookie.put('token', this.membershipModel.token);
  }

  logout() {
    // this.loggedIn.next(false);
    this.removeCredentials();
    this.router.navigate(['/auth/login']);
  }

  removeCredentials() {
    localStorage.clear();
    this.cookie.remove('userdetails');
    this.cookie.remove('token');
  }

  getUserDetails(key: string) {
    const userObj = this.cookie.getObject('userdetails');
    if (userObj == undefined) {
      return '';
    }
    else {
      return userObj[key];
    }
  }
}




