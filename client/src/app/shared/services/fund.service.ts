import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { GetFundsRequest } from '../_requests/getFundsRequest';
import { Fund } from '../_models/fund.model';

@Injectable({
  providedIn: 'root'
})
export class FundService {

  http = inject(HttpClient)
  apiUrl = environment.apiUrl;
  constructor() { }
  getFundsByCollageId(request: GetFundsRequest) {
    let params = new HttpParams();
    if (request.collageId) params = params.set('collageId', request.collageId);
    if (request.fundName) params = params.set('fundName', request.fundName);
    if (request.fundCode) params = params.set('fundCode', request.fundCode);
    if (request.id) params = params.set('id', request.id);
    return this.http.get(`${this.apiUrl}funds`, { params });
  }
  postFund(fund: Fund) {
    return this.http.post(`${this.apiUrl}funds`, fund);
  }
  putFund(id: number, fund: Fund) {
    return this.http.put(`${this.apiUrl}funds/${id}`, fund);
  }
  deleteFund(id: number) {
    return this.http.delete(`${this.apiUrl}funds/${id}`);
  }
}
