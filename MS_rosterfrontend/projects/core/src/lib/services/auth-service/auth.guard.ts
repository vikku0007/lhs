import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { MembershipService } from '../membership-service/membership.service';

@Injectable({
  providedIn: 'root'
})
export class AuthAdmin implements CanActivate {

  constructor(private membershipService: MembershipService, private route: Router) {


  }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.membershipService.isLoggedIn()) {
      // if (this.membershipService.getUserDetails('userRole').indexOf(1) >= 0) {
      //   return true;
      // }
      if (this.membershipService.getUserDetails('userRole') !== undefined) {
        if (this.membershipService.getUserDetails('userRole') == '1' ||
          this.membershipService.getUserDetails('userRole') == '10') {
          return true;
        } else {
          this.route.navigate(['/auth/login']);

        }
      } else if (this.membershipService.membershipModel.userRole == '1' ||
        this.membershipService.membershipModel.userRole == '10') {
        return true;
      } else {
        this.route.navigate(['/auth/login']);
      }
      // if (this.membershipService.membershipModel.userRole == '1' || this.membershipService.getUserDetails('userRole').indexOf('1') >= 0) {
      //   return true;
      // }
      // else {
      //   this.route.navigate(['/auth/login']);
      // }
    }
    return false;
  }
}
