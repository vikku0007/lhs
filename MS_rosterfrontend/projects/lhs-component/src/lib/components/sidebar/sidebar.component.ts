import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MembershipService } from 'projects/core/src/projects';

@Component({
  selector: 'lhs-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  roleId: number = 0;

  constructor(private router: Router, private membershipService: MembershipService) { }

  ngOnInit(): void {
    this.roleId = this.membershipService.getUserDetails('userRole');
  }
  openlocation() {
    this.router.navigate(['/admin/location']);
  }
  openmaster() {
    this.router.navigate(['/admin/master-entries']);
  }
  openCommunication() {
    this.router.navigate(['/employee/communication-list']);
  }
  openAccident() {
    this.router.navigate(['/employee/accident-list']);
  }
  openAppraisal() {
    this.router.navigate(['/employee/appraisal-list']);
  }
  openCompliance() {
    this.router.navigate(['/employee/compliance-list']);
  }
  openLeave() {
    this.router.navigate(['/employee/leave-list']);
  }
  openStaff() {
    this.router.navigate(['/employee/staffwarning-list']);
  }
  openEmployee() {
    this.router.navigate(['/employee']);
  }

  // Client methods starts
  openClient() {
    this.router.navigate(['/client']);
  }

  openClientAccidents() {
    this.router.navigate(['/client/accident-list']);
  }

  openMedicalHistory() {
    this.router.navigate(['/client/medical-history-list']);
  }

  openProgressNotes() {
    this.router.navigate(['/client/progress-notes-list']);
  }

  openClientDocuments() {
    this.router.navigate(['/client/document-list']);
  }

  openRoster() {
    this.router.navigate(['/roster/scheduler']);
  }

  openDashboard() {
    this.router.navigate(['admin/dashboard']);
  }
  openpayroll() {
    this.router.navigate(['/admin/employee-payroll']);
  }
  rejectedShiftPayroll() {
    this.router.navigate(['/admin/employee-payroll/employee-rejected-payroll']);
  }
  incompleteShiftPayroll() {
    this.router.navigate(['/admin/employee-payroll/employee-incomplete-payroll']);
  }
  payrollInMyOb(){
    this.router.navigate(['/admin/employee-payroll/payroll-in-myob']);
  }
  openpublicholiday() {
    this.router.navigate(['/admin/public-holiday']);
  }
  openAuditLog() {
    this.router.navigate(['/admin/audit-log']);
  }
  openTodoItem() {
    this.router.navigate(['/admin/to-do-item']);
  }
  openTimesheet() {
    this.router.navigate(['/admin/timesheet']);
  }
  openHourlyTimesheet() {
    this.router.navigate(['/admin/timesheet/hourly-timesheet']);
  }
  openglobalpayrate() {
    this.router.navigate(['/admin/global-payrate']);
  }
  openserviceprice() {
    this.router.navigate(['/admin/upload-serviceprice']);
  }
  openaddlocation() {
    this.router.navigate(['/admin/add-location']);
  }
}
