import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  getRoles() {
    return this.http.get<any>(`${this.apiUrl}auth/roles`);
  }
 
}
