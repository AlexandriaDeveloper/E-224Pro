import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { GetDailiesRequest } from '../_requests/getDailiesRequest';
import { StringToDateOnlyProviderService } from '../_helper/string-to-date-only-provider.service';
import { Daily, ReportRequest } from '../_models/Daily.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DailiesService {

  apiUrl = environment.apiUrl
  dateProvider = inject(StringToDateOnlyProviderService);

  constructor(private http: HttpClient) { }
  getDailies(getDailiesRequest: GetDailiesRequest) {
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
    return this.http.get(`${this.apiUrl}dailies`, { params: parms });
  }

  //add daily
  addDaily(daily: Daily) {
    return this.http.post(`${this.apiUrl}dailies/create`, daily);

  }
  updateDaily(id: number, daily: Daily) {
    return this.http.put(`${this.apiUrl}dailies/update/${id}`, daily);
  }
  deleteDaily(id: any) {
    return this.http.delete(`${this.apiUrl}dailies/delete/${id}`);
  }

  downloadDailieRportPdf(getDailiesReportRequest: ReportRequest) {
    var params = new HttpParams();

    if (getDailiesReportRequest.startDate != null) {
      params = params.append('startDate', this.dateProvider.stringToDateOnlyProvider(getDailiesReportRequest.startDate.toString()));
    }
    if (getDailiesReportRequest.endDate != null) {
      params = params.append('endDate', this.dateProvider.stringToDateOnlyProvider(getDailiesReportRequest.endDate.toString()));
    }
    if (getDailiesReportRequest.dailyType != null) {
      params = params.append('dailyType', getDailiesReportRequest.dailyType);
    }
    if (getDailiesReportRequest.entryType != null) {
      params = params.append('entryType', getDailiesReportRequest.entryType);
    }
    if (getDailiesReportRequest.collageId != null) {
      params = params.append('collageId', getDailiesReportRequest.collageId);
    }
    if (getDailiesReportRequest.fundId != null) {
      params = params.append('fundId', getDailiesReportRequest.fundId);
    }

    return this.http.get(`${this.apiUrl}reports/ReportDailiesPdf`, {
      responseType: 'blob' as 'json',
      params: params
    });

  }
  uploadExcelForm(formData: FormData, dailyId: number) {
    return this.http.post(`${this.apiUrl}dailies/${dailyId}/UploadExcelForm`, formData, {
      responseType: 'blob' as 'json',
    });
  }


}
