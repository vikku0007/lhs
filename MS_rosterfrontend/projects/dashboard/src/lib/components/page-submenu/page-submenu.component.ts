import { Component, OnInit, Input, Output, OnChanges, SimpleChanges, EventEmitter } from '@angular/core';
import { EmployeeDetails } from '../../viewmodel/employee-details';
import { EmployeeJobProfile } from '../../viewmodel/employee-jobprofile';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-page-submenu',
  templateUrl: './page-submenu.component.html',
  styleUrls: ['./page-submenu.component.scss']
})
export class PageSubmenuComponent implements OnInit, OnChanges {
  

  employeeId: number = 0;

  constructor(private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      this.employeeId = Number(params.Id);
    });
  }

  ngOnChanges(changes: SimpleChanges): void {    
   // this.employeeId = this.empPrimaryInfo.id;
  }

  ngOnInit(): void {

  }

}
