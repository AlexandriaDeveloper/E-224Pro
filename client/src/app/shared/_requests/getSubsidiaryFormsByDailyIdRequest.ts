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
}