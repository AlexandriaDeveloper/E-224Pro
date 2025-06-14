// {
//     "id": 2000,
//     "formName": "Form 200",
//     "collageId": 1,
//     "collageName": "طب",
//     "fundId": 2,
//     "fundName": "خدمة تعليمية طب",
//     "num224": "1990",
//     "auditorName": "Mohamed Ali",
//     "details": "Hello world",
//     "num55": "1990",
//     "dailyId": 11,
//     "totalCredit": 130726,
//     "totalDebit": 130726,
//     "totalCredit2": null,
//     "totalDebit2": null,
//     "isBalanced": true,
//     "formDetailsDtos": null
// }

import { Param } from "./Param"

export class AddFormRequest {
    public Id?: number
    public FormName?: string
    public CollageId?: number
    public CollageName?: string
    public FundId?: number
    public FundName?: string
    public Num224?: string
    public AuditorName?: string
    public Details?: string
    public Num55?: string
    public DailyId?: number

    public FormDetailsDtos?: Array<AddFormDetailsRequest>
}

export class AddFormDetailsRequest {
    public FormId?: number
    public AccountId?: number
    public Debit?: number
    public Credit?: number
    public AccountType?: string
}

