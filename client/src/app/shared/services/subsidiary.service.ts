import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { GetSubsidiaryFormsByDailyIdRequest } from '../_requests/getSubsidiaryFormsByDailyIdRequest';

@Injectable({
  providedIn: 'root'
})
export class SubsidiaryService {
  apiUrl = environment.apiUrl
  http = inject(HttpClient)
  constructor() { }

  GetSubsidaryDailyFormsByDailyId(subsidaryId: number, param: GetSubsidiaryFormsByDailyIdRequest) {
    let params = new HttpParams();

    if (param.Id) params = params.append('Id', param.Id.toString());
    if (param.AccountId) params = params.append('AccountId', param.AccountId.toString());
    if (param.SubAccountId) params = params.append('SubAccountId', param.SubAccountId.toString());
    if (param.DailyId) params = params.append('DailyId', param.DailyId.toString());
    if (param.FormDetailsId) params = params.append('FormDetailsId', param.FormDetailsId.toString());
    if (param.CollageId) params = params.append('CollageId', param.CollageId.toString());
    if (param.FundId) params = params.append('FundId', param.FundId.toString());


    return this.http.get(`${this.apiUrl}SubsidiaryJournal/${subsidaryId}`, { params });

  }

}
