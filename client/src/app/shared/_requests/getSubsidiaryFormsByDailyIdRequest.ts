import { Param } from "./Param";

export class GetSubsidiaryFormsByDailyIdRequest extends Param {

    public id?: number;
    public accountId?: number;
    public subAccountId?: number;
    public dailyId?: number;
    public formDetailsId?: number;
    public dailyType?: string;
    public collageId?: number;
    public fundId?: number;
    public num55: string;
    public num224: string;
    public startDate?: Date;
    public endDate?: Date;
    public entryType?: number;
    public formName?: string;
    public auditorName?: string;
    public isBalanced?: any;
}


export class SubsidaryToExcelRequest {
    //make it camel case
    public id?: number; //make it camel case
    public accountId?: number; //make it camel case

    public dailyId?: number; //make it camel case
    public fundId?: number; //make it camel case
    public collageId?: number; //make it camel case


}