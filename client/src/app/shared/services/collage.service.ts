import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CollageService {
  http = inject(HttpClient)
  apiUrl = environment.apiUrl;
  constructor() { }
  getCollages() {

    return this.http.get(`${this.apiUrl}collages`);
  }
}
