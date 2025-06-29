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
    formDetailsDto: Array<FormDetailDto> = new Array<FormDetailDto>();
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



