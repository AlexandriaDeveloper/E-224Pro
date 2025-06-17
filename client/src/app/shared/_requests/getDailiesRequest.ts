import { Param } from "./Param";

export class GetDailiesRequest extends Param {
    name?: string = null;
    startDate? = null;
    endDate? = null;
    dailyType?: string = null;
    accountItem?: string = null;
    FundId: any;
    accountId?: number;
    dailyId?: number
}