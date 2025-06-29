import { Param } from "./Param";

export class GetDailiesRequest extends Param {
    name?: string = null;
    startDate? = null;
    endDate? = null;
    dailyType?: string = null;
    accountItem?: string = null;
    FundId?: number;
    collageId?: number;
    accountId?: number;
    entityType?: number = null;
    dailyId?: number
}