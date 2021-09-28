import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { Constants } from 'projects/Admin Area/admin/src/lib/config/constants';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private apiService: ApiService) { }


  getAllMasterEntriesList(data) {
    return this.apiService.post(Constants.GetAllMasterEntries, data).pipe(map(res => {
      return res;
    }))
  }
  AddMasterentries(data) {
    return this.apiService.post(Constants.AddMasterEntries, data).pipe(map(res => {
      return res;
    }))
  }
  UpdateMasterentries(data) {
    return this.apiService.post(Constants.UpdateMasterEntries, data).pipe(map(res => {
      return res;
    }))
  }
  UpdateMasterEntriesStatus(data) {
    return this.apiService.post(Constants.UpdateMasterActiveStatus, data).pipe(map(res => {
      return res;
    }))
  }
  AddPublicHoliday(data) {
    return this.apiService.post(Constants.AddPublicHoliday, data).pipe(map(res => {
      return res;
    }))
  }
  DeletePublicHoliday(data) {
    return this.apiService.post(Constants.DeletePublicHoliday, data).pipe(map(res => {
      return res;
    }))
  }
  UpdatePublicHoliday(data) {
    return this.apiService.post(Constants.UpdatePublicHoliday, data).pipe(map(res => {
      return res;
    }))
  }
  GetAllPublicHoliday(data) {
    return this.apiService.post(Constants.GetAllPublicHoliday, data).pipe(map(res => {
      return res;
    }))
  }
  AddToDolistItem(data) {
    return this.apiService.post(Constants.AddToDolistItem, data).pipe(map(res => {
      return res;
    }))
  }
  DeleteToDolistItem(data) {
    return this.apiService.post(Constants.DeleteToDolistItem, data).pipe(map(res => {
      return res;
    }))
  }
  UpdateToDolistItem(data) {
    return this.apiService.post(Constants.UpdateToDolistItem, data).pipe(map(res => {
      return res;
    }))
  }
  GetShiftItemList(data) {
    return this.apiService.post(Constants.GetShiftItem,data, null).pipe(map(res => {
      return res;
    }))
  }
  GetShiftType() {
    return this.apiService.get(Constants.getShifttype).pipe(map(res => {
      return res;
    }))
  }
  getUserActivitylog(data) {
    return this.apiService.post(Constants.getUserActivitylog,data,null).pipe(map(res => {
      return res;
    }))
  }
  getpayrateInfo(data) {
    return this.apiService.post(Constants.getpayrateInfo,data,null).pipe(map(res => {
      return res;
    }))
  }
  EditpayrateInfo(data) {
    return this.apiService.post(Constants.EditpayrateInfo,data,null).pipe(map(res => {
      return res;
    }))
  }
  Uploadserviceprice(data) {
    return this.apiService.post(Constants.Uploadserviceprice, data, null).pipe(map(res => {
      return res;
    }));
  }
  GetServiceRate(data) {
    return this.apiService.post(Constants.GetServiceRate, data, null).pipe(map(res => {
      return res;
    }));
  }

}
