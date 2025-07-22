import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { GetFormRequest, SearchFormRequest } from '../_requests/getFormRequest';
import { StringToDateOnlyProviderService } from '../_helper/string-to-date-only-provider.service';

@Injectable({
  providedIn: 'root'
})
export class FormService {
  dateProvider = inject(StringToDateOnlyProviderService);
  deleteForm(formId: number) {
    return this.http.delete(`${this.apiUrl}forms/${formId}`);
  }
  getFormDetails(formId: number) {

    return this.http.get(`${this.apiUrl}FormDetails/${formId}`);
  }
  getFormWithDetails(formId: number) {
    let params = new HttpParams();
    params = params.append('formId', formId.toString());
    return this.http.get(`${this.apiUrl}forms/formWithDetail`, { params: params });
  }

  apiUrl = environment.apiUrl
  http = inject(HttpClient)
  constructor() { }
  //Get Forms 
  getForms(getFormParam: SearchFormRequest): any {
    let params = new HttpParams();

    if (getFormParam.id != null) {
      params = params.append('id', getFormParam.id.toString());
    }
    if (getFormParam.formName != null) {
      params = params.append('formName', getFormParam.formName);
    }
    if (getFormParam.collageId != null) {
      params = params.append('collageId', getFormParam.collageId.toString());
    }
    if (getFormParam.fundId != null) {
      params = params.append('fundId', getFormParam.fundId.toString());
    }
    if (getFormParam.formNum224 != null) {
      params = params.append('formNum224', getFormParam.formNum224);
    }
    if (getFormParam.formNum55 != null) {
      params = params.append('formNum55', getFormParam.formNum55);
    }
    if (getFormParam.dailyId != null) {
      params = params.append('dailyId', getFormParam.dailyId.toString());
    }
    if (getFormParam.auditorName != null) {
      params = params.append('auditorName', getFormParam.auditorName);
    }
    if (getFormParam.details != null) {
      params = params.append('details', getFormParam.details);
    }
    if (getFormParam.entryType != null) {
      params = params.append('entryType', getFormParam.entryType.toString());
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
    return this.http.get(`${this.apiUrl}Reports/ReportDailyPdf`, {
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
  searchForm(searchForm: SearchFormRequest) {
    console.log(searchForm);

    let params = new HttpParams();
    if (searchForm.pageIndex != null) {
      params = params.append('pageIndex', searchForm.pageIndex.toString());
    }
    if (searchForm.pageSize != null) {
      params = params.append('pageSize', searchForm.pageSize.toString());
    }
    if (searchForm.formName != null) {
      params = params.append('formName', searchForm.formName);
    }
    if (searchForm.formNum224 != null) {
      params = params.append('formNum224', searchForm.formNum224);
    }
    if (searchForm.formNum55 != null) {
      params = params.append('formNum55', searchForm.formNum55);
    }
    if (searchForm.collageId != null) {
      params = params.append('collageId', searchForm.collageId.toString());
    }
    if (searchForm.collageName != null) {
      params = params.append('collageName', searchForm.collageName);
    }
    if (searchForm.accountId != null) {
      params = params.append('accountId', searchForm.accountId.toString());
    }
    if (searchForm.accountName != null) {
      params = params.append('accountName', searchForm.accountName);
    }
    if (searchForm.fundId != null) {
      params = params.append('fundId', searchForm.fundId.toString());
    }
    if (searchForm.fundName != null) {
      params = params.append('fundName', searchForm.fundName);
    }
    if (searchForm.dailyId != null) {
      params = params.append('dailyId', searchForm.dailyId.toString());
    }
    if (searchForm.auditorName != null) {
      params = params.append('auditorName', searchForm.auditorName);
    }
    if (searchForm.details != null) {
      params = params.append('details', searchForm.details);
    }
    // if (searchForm.AccountType != null) {
    //   params = params.append('accountType', searchForm.AccountType);
    // }
    if (searchForm.entryType != null) {
      params = params.append('entryType', searchForm.entryType.toString());
    }
    if (searchForm.dailyType != null) {
      params = params.append('dailyType', searchForm.dailyType);
    }
    if (searchForm.startFrom != null) {
      params = params.append('startFrom', this.dateProvider.stringToDateOnlyProvider(searchForm.startFrom.toString()));
    }
    if (searchForm.endTo != null) {
      params = params.append('endTo', this.dateProvider.stringToDateOnlyProvider(searchForm.endTo.toString()));
    }
    if (searchForm.sort != null) {
      params = params.append('sort', searchForm.sort);
    }
    if (searchForm.direction != null) {
      params = params.append('direction', searchForm.direction);
    }
    return this.http.get(`${this.apiUrl}forms/search`, { params: params });
  }


}
