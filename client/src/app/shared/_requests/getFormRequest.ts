

import { Param } from "./Param";

export class GetFormRequest extends Param {

    //change to camelCase
    public id?: number;

    public formName?: string;
    public collageId?: number;
    public fundId?: number;
    public num224?: string;
    public num55?: string;
    public dailyId?: number;
    public auditorName?: string;
    public details?: string;
    public entryType?: number; // Assuming EntryType is an enum, you can use a number to represent it
}


export class SearchFormRequest extends Param {

    //i want properties to be camelCase
    // and i want to use Date for StartFrom and EndTo
    public id?: number;
    public formId?: number;
    public formName?: string;
    public formNum224?: string;
    public formNum55?: string;
    public collageId?: number;
    public fundId?: number;
    public dailyId?: number;
    public auditorName?: string;
    public details?: string;
    public collageName?: string;
    public fundName?: string;
    public accountId?: number;
    public accountName?: string;
    public entryType?: number; // Assuming EntryType is an enum, you can use a number to represent it
    //   public AccountNumber?: string; // Uncomment if needed


    public dailyType?: string; // Assuming DailyType is a string, adjust as necessary
    public startFrom?: Date; // Assuming you want to use Date for StartFrom
    public endTo?: Date; // Assuming you want to use Date for EndTo
}