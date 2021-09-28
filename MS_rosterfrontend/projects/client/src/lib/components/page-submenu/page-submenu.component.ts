import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'lib-page-submenu',
  templateUrl: './page-submenu.component.html',
  styleUrls: ['./page-submenu.component.scss']
})
export class PageSubmenuComponent implements OnInit {
  clientId: number;

  constructor(private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      this.clientId = Number(params.Id);
    });
  }

  ngOnInit(): void {
  }

}
