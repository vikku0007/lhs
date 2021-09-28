import { Injectable } from '@angular/core';
import { ApiService } from 'projects/core/src/projects';
import { Constants } from 'projects/Admin Area/admin/src/lib/config/constants';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class LocationService {

  constructor(private apiService: ApiService) { }

  getLocationList(data) {
    return this.apiService.post(Constants.getAllLocation, data).pipe(map(res => {
      return res;
    }))
  }

  addLocationDetails(data) {
    return this.apiService.post(Constants.addLocationDetails, data, null).pipe(map(res => {
      return res;
    }))
  }
  EditLocationDetails(data) {
    return this.apiService.post(Constants.UpdateLocationDetails, data, null).pipe(map(res => {
      return res;
    }  ))
  }
  DeleteLocationDetails(data) {
    return this.apiService.post(Constants.DeleteLocation, data, null).pipe(map(res => {
      return res;
    }))
  }
  getLocationDetailbyId(data) {
    return this.apiService.post(Constants.getLocationDetailById, data).pipe(map(res => {
      return res;
    }))
  }
  addLatLongLocationDetails(data) {
    return this.apiService.post(Constants.addLatLongLocationDetails, data, null).pipe(map(res => {
      return res;
    }))
  }
  editLatLongLocationDetails(data) {
    return this.apiService.post(Constants.editLatLongLocationDetails, data, null).pipe(map(res => {
      return res;
    }))
  }
}
