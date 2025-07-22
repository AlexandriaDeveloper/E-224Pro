export class FormDto {
    id: number;
    formName: string;
    collageId: number;
    fundId: number;
    num224: string;
    num55: string;
    dailyId: number;
    auditorName: string;
    details: string;
    entryType: number; // Added entryType field
    formDetailsDtos: FormDetailDto[] = [];
}
export class FormDetailDto {
    id: number;
    formId: number;
    accountId: number;
    debit: number;
    credit: number;
    accountType: string;
}


export class SubsidaryFormDto {
    id: number;
    formName: string;
    collageId: number;
    fundId: number;
    num224: number;
    num55: number;
    dailyId: number;
    auditorName: string;
    details: string;
    formDetailsDto: Array<SubsidaryFormDetailDto> = new Array<SubsidaryFormDetailDto>();
}
export class SubsidaryFormDetailDto {
    id: number;
    formId: number;
    accountId: number;
    debit: number;
    credit: number;
    accountType: string;
}

export class SearchFormModel {
    /*
    {
        "id": 104,
        "formName": "ملف 99",
        "collageName": "طب",
        "fundName": "خدمة تعليمية طب",
        "num224": "99",
        "num55": "99",
        "dailyId": 3,
        "dailyName": "يوميه 20-60-2025",
        "dailyDate": "2025-06-20",
        "dailyType": "Payroll",
        "auditorName": "محمد على شريف",
        "details": ""
    }
    */

    // Define the properties for the search request as camal case
    id: number;
    formName: string;
    collageName: string;
    fundName: string;
    num224: string;
    num55: string;
    dailyId: number;
    dailyName: string;
    dailyDate: string;
    dailyType: string;
    auditorName: string;
    details: string;
}



