import { Component, OnInit, ViewChild } from '@angular/core';
import { ClientService } from '../../services/client.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ShiftInfoViewModel } from 'projects/roster/src/lib/viewmodel/roster-shift-info-viewModel';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'lib-current-shift',
  templateUrl: './current-shift.component.html',
  styleUrls: ['./current-shift.component.scss']
})
export class CurrentShiftComponent implements OnInit {
  clientId: number = 0;
  responseModel: ResponseModel = {};
  shiftModel: ShiftInfoViewModel[] = [];
  displayedColumnsRequired: string[] = ['sr', 'employeeName', 'location', 'startDate', 'endDate', 'startTime', 'endTime', 'action'];
  dataSourceRequired = new MatTableDataSource(this.shiftModel);
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private clientService: ClientService, private route: ActivatedRoute) {
    this.route.paramMap.subscribe((params: any) => {
      this.clientId = Number(params.params.id);
    });
  }

  ngOnInit(): void {
    this.dataSourceRequired.sort = this.sort;
    this.getClientShiftList();
  }

  getClientShiftList() {
    this.clientService.getCurrentShiftList({ id: this.clientId }).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.shiftModel = this.responseModel.responseData;
          this.dataSourceRequired = new MatTableDataSource(this.shiftModel);
          break;

        default:
          break;
      }
    })
  }

}
