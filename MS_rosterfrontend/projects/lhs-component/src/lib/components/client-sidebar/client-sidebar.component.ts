import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MembershipService } from 'projects/core/src/projects';

@Component({
  selector: 'lib-client-sidebar',
  templateUrl: './client-sidebar.component.html',
  styleUrls: ['./client-sidebar.component.scss']
})
export class ClientSidebarComponent implements OnInit {

  clientId: number;
  constructor(private router: Router, private membershipService: MembershipService) { }

  ngOnInit(): void {
    this.clientId = this.membershipService.getUserDetails('employeeId');
  }

  openDashboard() {
    this.router.navigate(['client/dashboard', this.clientId]);
  }

  openRoster() {
    this.router.navigate(['client/roster', this.clientId]);
  }

}
