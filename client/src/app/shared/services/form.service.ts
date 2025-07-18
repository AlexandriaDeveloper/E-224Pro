import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { GetFormRequest } from '../_requests/getFormRequest';

@Injectable({
  providedIn: 'root'
})
export class FormService {
  deleteForm(formId: number) {
    return this.http.delete(`${this.apiUrl}forms/${formId}`);
  }
  getFormDetails(formId: number) {

    return this.http.get(`${this.apiUrl}FormDetails/${formId}`);
  }

  apiUrl = environment.apiUrl
  http = inject(HttpClient)
  constructor() { }
  //Get Forms 
  getForms(getFormParam: GetFormRequest) {
    let params = new HttpParams();

    if (getFormParam.Id != null) {
      params = params.append('Id', getFormParam.Id.toString());
    }
    if (getFormParam.FormName != null) {
      params = params.append('FormName', getFormParam.FormName);
    }
    if (getFormParam.CollageId != null) {
      params = params.append('CollageId', getFormParam.CollageId.toString());
    }
    if (getFormParam.FundId != null) {
      params = params.append('FundId', getFormParam.FundId.toString());
    }
    if (getFormParam.Num224 != null) {
      params = params.append('Num224', getFormParam.Num224);
    }
    if (getFormParam.Num55 != null) {
      params = params.append('Num55', getFormParam.Num55);
    }
    if (getFormParam.DailyId != null) {
      params = params.append('DailyId', getFormParam.DailyId.toString());
    }
    if (getFormParam.AuditorName != null) {
      params = params.append('AuditorName', getFormParam.AuditorName);
    }
    if (getFormParam.Details != null) {
      params = params.append('Details', getFormParam.Details);
    }
    if (getFormParam.EntryType != null) {
      params = params.append('EntryType', getFormParam.EntryType.toString());
    }
    if (getFormParam.pageIndex != null) {
      params = params.append('pageIndex', getFormParam.pageIndex.toString());
    }
    if (getFormParam.pageSize != null) {
      params = params.append('pageSize', getFormParam.pageSize.toString());
    }

    return this.http.get(`${this.apiUrl}forms`, { params: params });
  }

  addForm(value: any) {
    return this.http.post(this.apiUrl + 'forms/AddForm', value);
  }
  updateForm(id: number, value: any) {

    return this.http.put(this.apiUrl + 'forms/update/' + id, value);
  }

  downloadDailyPdfFormTemplate(dailyId: number) {
    let params = new HttpParams();
    params = params.append('dailyId', dailyId.toString());
    return this.http.get(`${this.apiUrl}Reports/ReportPdf`, {
      responseType: 'blob' as 'json',
      params: params
    });
  }

  downloadDailyExcelFormTemplate(result) {
    return this.http.post(`${this.apiUrl}Forms/DownloadTemplateExcelSheet`, result
      , {
        responseType: 'blob' as 'json',
      },

    );
  }

  downloadDailyExcelForms(dailyId: number) {
    return this.http.post(`${this.apiUrl}Forms/DownloadExcelSheet/${dailyId}`, {}
      , {
        responseType: 'blob' as 'json',
      },

    );
  }


}
