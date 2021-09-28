import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  constructor() { }

  start() {
    document.getElementById('loader').style.display = 'flex';
  }

  stop() {
    document.getElementById('loader').style.display = 'none';
  }
}
