export class FormDto {
    id: number;
    formName: string;
    collageId: number;
    fundId: number;
    num224: number;
    num55: number;
    dailyId: number;
    auditorName: string;
    details: string;
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