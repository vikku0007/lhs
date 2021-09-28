import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MembershipService } from 'projects/core/src/projects';

@Component({
  selector: 'lib-admin-sidebar',
  templateUrl: './admin-sidebar.component.html',
  styleUrls: ['./admin-sidebar.component.scss']
})
export class AdminSidebarComponent implements OnInit {
  employeeId: number;
  constructor(private router: Router, private membershipService: MembershipService) { }

  ngOnInit(): void {
    this.employeeId = this.membershipService.getUserDetails('employeeId');
  }

  openShift() {
    this.router.navigate(['employee/dashboard/shift-list', this.employeeId]);
  }  
  
  openLeave() {
    this.router.navigate(['employee/dashboard/leave', this.employeeId]);
  }

  openRoster() {
    this.router.navigate(['employee/roster/view', this.employeeId]);
  }
  
  openIncident() {
    this.router.navigate(['employee/others/incident-reported', this.employeeId]);
  }

  openComplaints() {
    this.router.navigate(['employee/others/complaints', this.employeeId]);
  }

  openWarning() {
    this.router.navigate(['employee/others/warning', this.employeeId]);
  }

  openEmpDoc() {
    this.router.navigate(['employee/others/document', this.employeeId]);
  }

  openDashboard() {
    this.router.navigate(['employee/dashboard', this.employeeId]);
  }

  openEmpCommunication() {
    this.router.navigate(['employee/others/communication', this.employeeId]);
  }
  openEmployeeDetail() {
    this.router.navigate(['employeedetail/employeedetails', this.employeeId]);
  }
  openLeaveApprove() {
    this.router.navigate(['employee/dashboard/leaveapprove', this.employeeId]);
  }
}
