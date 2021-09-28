import { Component, OnInit } from '@angular/core';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ShiftDetailViewModel } from '../../view-models/shift-info-view-model';
import { ActivatedRoute } from '@angular/router';
import { ClientService } from '../../services/client.service';
import { NotificationService } from 'projects/core/src/projects';

@Component({
  selector: 'lib-view-shift',
  templateUrl: './view-shift.component.html',
  styleUrls: ['./view-shift.component.scss']
})
export class ViewShiftComponent implements OnInit {

  response: ResponseModel = {};
  shiftId: number = 0;
  shiftInfo: ShiftDetailViewModel = {}

  constructor(private route: ActivatedRoute, private clientService: ClientService, private notificationService: NotificationService) {
    this.route.paramMap.subscribe((params: any) => {
      this.shiftId = Number(params.params.id);
    });
  }

  ngOnInit(): void {
    this.getShiftInfo();
  }

  getShiftInfo() {
    const data = {
      id: this.shiftId
    }
    this.clientService.getShiftDetail(data).subscribe((res => {
      if (res) {
        this.response = res;
        this.shiftInfo = this.response.responseData || {}
      } else {
        this.notificationService.Error({ message: 'Something went wrong! Shift not found', title: null });
      }
    }));
  }


}
