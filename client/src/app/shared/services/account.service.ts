import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  apiUrl = environment.apiUrl
  http = inject(HttpClient)
  constructor() { }
  getAccounts(getAccountRequest: any) {
    let params = new HttpParams();

    if (getAccountRequest.AccountName != null) {
      params = params.append('AccountName', getAccountRequest.AccountName);
    }
    if (getAccountRequest.AccountNumber != null) {
      params = params.append('AccountNumber', getAccountRequest.AccountNumber);
    }
    if (getAccountRequest.Id != null) {
      params = params.append('Id', getAccountRequest.Id.toString());
    }
    return this.http.get(`${this.apiUrl}accounts`, { params: params });
  }

}
