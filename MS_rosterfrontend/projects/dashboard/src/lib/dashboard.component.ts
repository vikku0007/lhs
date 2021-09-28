import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'lib-dashboard',
  // templateUrl: './dashboard.component.html',
  // styleUrls: ['./dashboard.component.scss']
  template: `
  <lhs-header></lhs-header>
  <lhs-sidebar></lhs-sidebar>
    <router-outlet></router-outlet>
  `,
  styles: [
  ]
})
export class DashboardComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
