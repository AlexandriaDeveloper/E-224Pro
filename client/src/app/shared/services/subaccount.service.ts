import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { GetSubAccountRequest } from '../_requests/GetSubAccountRequest';

@Injectable({
  providedIn: 'root'
})
export class SubaccountService {
  apiUrl = environment.apiUrl
  http = inject(HttpClient)
  constructor() { }

  getAccounts(accountId: number, request: GetSubAccountRequest) {
    let params = new HttpParams();

    if (request.ParentAccountName != null) {
      params = params.append('ParentAccountName', request.ParentAccountName);
    }
    if (request.ParentAccountNumber != null) {
      params = params.append('ParentAccountNumber', request.ParentAccountNumber);
    }
    if (request.SubAccountName != null) {
      params = params.append('SubAccountName', request.SubAccountName);
    }
    if (request.SubAccountNumber != null) {
      params = params.append('SubAccountNumber', request.SubAccountNumber);
    }
    if (request.Id != null) {
      params = params.append('Id', request.Id.toString());
    }
    if (request.AccountId != null) {
      params = params.append('AccountId', request.AccountId.toString());
    }

    return this.http.get(`${this.apiUrl}subAccount/${accountId}`, { params: params });
  }

}
