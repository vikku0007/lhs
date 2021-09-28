import { Component, OnInit } from '@angular/core';
import { MembershipService } from 'projects/core/src/projects';
import { CommonService } from 'projects/lhs-service/src/projects';
import { ActivatedRoute } from '@angular/router';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'lhs-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  UserRole: any;
  Id: number;
  response: ResponseModel = {};
  imagedetails: ImageInfo = {};
  baseUrl: string = environment.baseUrl;
  constructor(private membershipService: MembershipService, private CommonService: CommonService, private route: ActivatedRoute) { }
  ngOnInit(): void {
    this.UserRole = this.membershipService.getUserDetails('userRole');
    this.Id = this.membershipService.getUserDetails('employeeId');
    this.getimageInfo();

  }

  logout() {
    this.membershipService.logout();
  }
  getimageInfo() {
    const data = {
      UserRole: this.UserRole,
      Id: this.Id
    }
    this.CommonService.getImageDetails(data).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.imagedetails = this.response.responseData[0];

      }
    });
  }


  //  myFunction: boolean = false;

  toggleCls() {
    var element = document.getElementById("sidebar-on-off");
    if (element) {
      element.classList.toggle("sidebar-open");
    }
  }

}


export interface ImageInfo {
  imageUrl?: string;
  fullName?: string;

}