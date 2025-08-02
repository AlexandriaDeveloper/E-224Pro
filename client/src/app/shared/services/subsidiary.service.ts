import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { GetSubsidiaryFormsByDailyIdRequest, SubsidaryToExcelRequest } from '../_requests/getSubsidiaryFormsByDailyIdRequest';
import { GetDailiesRequest } from '../_requests/getDailiesRequest';
import { StringToDateOnlyProviderService } from '../_helper/string-to-date-only-provider.service';

@Injectable({
  providedIn: 'root'
})
export class SubsidiaryService {
  exportSubsidaryDailyExcel(params: SubsidaryToExcelRequest) {
    let httpParams = new HttpParams();
    if (params.id) httpParams = httpParams.append('Id', params.id.toString());
    if (params.accountId) httpParams = httpParams.append('AccountId', params.accountId.toString());

    if (params.dailyId) httpParams = httpParams.append('DailyId', params.dailyId.toString());
    //fundId
    if (params.fundId) httpParams = httpParams.append('FundId', params.fundId.toString());
    //collage id 
    if (params.collageId) httpParams = httpParams.append('CollageId', params.collageId.toString());


    return this.http.get(`${this.apiUrl}SubsidiaryJournal/ExportSubsidiaryDailyToExcel`, { responseType: 'blob', params: httpParams });
  }
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

    if (param.id) params = params.append('Id', param.id.toString());
    if (param.accountId) params = params.append('AccountId', param.accountId.toString());
    if (param.subAccountId) params = params.append('SubAccountId', param.subAccountId.toString());
    // if (param.DailyId) params = params.append('DailyId', param.DailyId.toString());
    if (param.formDetailsId) params = params.append('FormDetailsId', param.formDetailsId.toString());
    if (param.collageId) params = params.append('CollageId', param.collageId.toString());
    if (param.fundId) params = params.append('FundId', param.fundId.toString());
    if (param.num55) params = params.set('num55', param.num55);
    if (param.num224) params = params.set('num224', param.num224);
    if (param.entryType) params = params.set('entryType', param.entryType.toString());
    if (param.isBalanced) params = params.set('isBalanced', param.isBalanced.toString());

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
  searchSubsiadaryDaily(param: GetSubsidiaryFormsByDailyIdRequest) {
    let params = new HttpParams();
    if (param.id) params = params.append('Id', param.id.toString());
    if (param.accountId) params = params.append('AccountId', param.accountId.toString());
    if (param.subAccountId) params = params.append('SubAccountId', param.subAccountId.toString());
    if (param.dailyId) params = params.append('DailyId', param.dailyId.toString());
    if (param.formDetailsId) params = params.append('FormDetailsId', param.formDetailsId.toString());
    if (param.collageId) params = params.append('CollageId', param.collageId.toString());
    if (param.fundId) params = params.append('FundId', param.fundId.toString());
    if (param.num55) params = params.set('num55', param.num55);
    if (param.num224) params = params.set('num224', param.num224);
    if (param.dailyType) params = params.set('dailyType', param.dailyType);
    if (param.entryType != null) params = params.set('entryType', param.entryType.toString());
    if (param.startDate != null) {
      params = params.append('startDate', this.dateProvider.stringToDateOnlyProvider(param.startDate.toString()));
    }
    if (param.endDate != null) {
      params = params.append('endDate', this.dateProvider.stringToDateOnlyProvider(param.endDate.toString()));
    }
  }

  downloadSubsidaryDailyPdf(param: GetSubsidiaryFormsByDailyIdRequest) {
    console.log(param);

    let params = new HttpParams();

    if (param.id) params = params.append('Id', param.id.toString());
    if (param.accountId) params = params.append('AccountId', param.accountId.toString());
    if (param.subAccountId) params = params.append('SubAccountId', param.subAccountId.toString());
    if (param.dailyId) params = params.append('DailyId', param.dailyId.toString());
    if (param.formDetailsId) params = params.append('FormDetailsId', param.formDetailsId.toString());
    if (param.collageId) params = params.append('CollageId', param.collageId.toString());
    if (param.fundId) params = params.append('FundId', param.fundId.toString());
    if (param.num55) params = params.set('num55', param.num55);
    if (param.num224) params = params.set('num224', param.num224);
    if (param.dailyType) params = params.set('dailyType', param.dailyType);
    if (param.entryType != null) params = params.set('entryType', param.entryType.toString());
    if (param.startDate != null) {
      params = params.append('startDate', this.dateProvider.stringToDateOnlyProvider(param.startDate.toString()));
    }
    if (param.endDate != null) {
      params = params.append('endDate', this.dateProvider.stringToDateOnlyProvider(param.endDate.toString()));
    }
    if (param.formName) params = params.set('formName', param.formName);
    if (param.auditorName) params = params.set('auditorName', param.auditorName);
    if (param.isBalanced !== undefined && param.isBalanced !== null) params = params.set('isBalanced', param.isBalanced.toString());

    return this.http.get(`${this.apiUrl}Reports/ReportSubsidiaryJournalPdf`, { responseType: 'blob' as 'json', params: params });
  }


  deleteSubsidaryDailyForm(formDetailsId: number) {
    return this.http.delete(`${this.apiUrl}SubsidiaryJournal/formDetailsId/${formDetailsId}`);
  }


  uploadSubsidaryDailyFormAsExcel(dailyId: number, accountId: number, file: File) {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post(`${this.apiUrl}SubsidiaryJournal/UploadSubsidiaryDailyToExcel/dailyId/${dailyId}/accountId/${accountId}`, formData, { responseType: 'blob' });

  }

}
