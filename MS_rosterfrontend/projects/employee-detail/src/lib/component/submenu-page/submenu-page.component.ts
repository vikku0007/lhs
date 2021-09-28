import { Component, OnInit, Input, Output, OnChanges, SimpleChanges, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MembershipService } from 'projects/core/src/projects';

@Component({
  selector: 'lib-submenu-page',
  templateUrl: './submenu-page.component.html',     
  styleUrls: ['./submenu-page.component.scss']
})
export class SubmenuPageComponent implements OnInit, OnChanges {
 
  employeeId: number = 0;
   constructor(private route: ActivatedRoute,private router: Router,private membershipService: MembershipService) {
   
  }

  ngOnChanges(changes: SimpleChanges): void {    
  }

  ngOnInit(): void {
    this.employeeId = this.membershipService.getUserDetails('employeeId');
  }
  openCommunication() {
    this.router.navigate(['employeedetail/communicationdetail', this.employeeId]);
  }
  openLeave() {
    this.router.navigate(['employeedetail/leave', this.employeeId]);
  }
  openStaffWarning() {
    this.router.navigate(['employeedetail/staffwarning', this.employeeId]);
  }
  openAccident() {
    this.router.navigate(['employeedetail/accidentdetail', this.employeeId]);
  }
  opendocumentlist() {
    this.router.navigate(['employeedetail/documentchecklist', this.employeeId]);
  }
}