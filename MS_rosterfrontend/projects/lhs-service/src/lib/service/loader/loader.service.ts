import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  constructor() { }

  start() {    
    document.getElementById('preloader').style.display = 'flex';
  }

  stop() {    
    document.getElementById('preloader').style.display = 'none';
  }
}
