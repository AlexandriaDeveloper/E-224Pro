import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { GetSubsidiaryFormsByDailyIdRequest } from '../_requests/getSubsidiaryFormsByDailyIdRequest';
import { GetDailiesRequest } from '../_requests/getDailiesRequest';
import { StringToDateOnlyProviderService } from '../_helper/string-to-date-only-provider.service';

@Injectable({
  providedIn: 'root'
})
export class SubsidiaryService {
  apiUrl = environment.apiUrl
  http = inject(HttpClient)
  dateProvider = inject(StringToDateOnlyProviderService);
  constructor() { }
  getSubsidaryDailies(accountId: number, getDailiesRequest: GetDailiesRequest) {
    var parms = new HttpParams();


    parms = parms.append('pageIndex', getDailiesRequest.pageIndex);
    parms = parms.append('pageSize', getDailiesRequest.pageSize);
    if (getDailiesRequest.sort != null) {
      parms = parms.append('sort', getDailiesRequest.sort);
    }
    if (getDailiesRequest.direction != null) {
      parms = parms.append('direction', getDailiesRequest.direction);
    }
    if (getDailiesRequest.accountId != null) {
      parms = parms.append('accountId', getDailiesRequest.accountId);
    }

    if (getDailiesRequest.name != null) {
      parms = parms.append('name', getDailiesRequest.name);
    }
    if (getDailiesRequest.startDate != null) {
      parms = parms.append('startDate', this.dateProvider.stringToDateOnlyProvider(getDailiesRequest.startDate));
    }
    if (getDailiesRequest.endDate != null) {
      parms = parms.append('endDate', this.dateProvider.stringToDateOnlyProvider(getDailiesRequest.endDate));
    }
    if (getDailiesRequest.dailyType != null) {

      parms = parms.append('dailyType', getDailiesRequest.dailyType);
    }
    if (getDailiesRequest.accountItem != null) {
      parms = parms.append('accountItem', getDailiesRequest.accountItem);
    }
    return this.http.get(`${this.apiUrl}SubsidiaryJournal/subId/${accountId}`, { params: parms });
  }

  GetSubsidaryDailyFormsByDailyId(accountId: number, dailyId: number, param: GetSubsidiaryFormsByDailyIdRequest) {
    let params = new HttpParams();

    if (param.Id) params = params.append('Id', param.Id.toString());
    if (param.AccountId) params = params.append('AccountId', param.AccountId.toString());
    if (param.SubAccountId) params = params.append('SubAccountId', param.SubAccountId.toString());
    // if (param.DailyId) params = params.append('DailyId', param.DailyId.toString());
    if (param.FormDetailsId) params = params.append('FormDetailsId', param.FormDetailsId.toString());
    if (param.CollageId) params = params.append('CollageId', param.CollageId.toString());
    if (param.FundId) params = params.append('FundId', param.FundId.toString());

    params = params.append('pageIndex', param.pageIndex);
    params = params.append('pageSize', param.pageSize);
    if (param.sort != null) {
      params = params.append('sort', param.sort);
    }
    if (param.direction != null) {
      params = params.append('direction', param.direction);
    }



    return this.http.get(`${this.apiUrl}SubsidiaryJournal/subId/${accountId}/dailyId/${dailyId}`, { params });

  } getSubsidartFormDetails(accountId: number, formDetailsId: number) {
    return this.http.get(`${this.apiUrl}SubsidiaryJournal/subId/${accountId}/formDetailsId/${formDetailsId}`);
  }

  addOrUpdateSubsidaryFormDetails(request: any) {
    return this.http.post(`${this.apiUrl}SubsidiaryJournal/AddOrUpdate`, request);
  }

}
