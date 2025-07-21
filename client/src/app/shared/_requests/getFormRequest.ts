/*
    public int? Id { get; set; }
    public string? FormName { get; set; }
    public int? CollageId { get; set; }
    public int? FundId { get; set; }
    public string? Num224 { get; set; }
    public string? Num55 { get; set; }
    public int? DailyId { get; set; }
    public string? AuditorName { get; set; }
    public string? Details { get; set; }
     */

import { Param } from "./Param";

export class GetFormRequest extends Param {
    public Id?: number;
    public FormName?: string;
    public CollageId?: number;
    public FundId?: number;
    public Num224?: string;
    public Num55?: string;
    public DailyId?: number;
    public AuditorName?: string;
    public Details?: string;
    public EntryType?: number; // Assuming EntryType is an enum, you can use a number to represent it
}

/**
 * 
 * public class SearchFormRequest : Param
{
    public int? Id { get; set; }
    public int? FormId { get; set; }
    public string? FormName { get; set; }
    public string? FormNum224 { get; set; }
    public string? FormNum55 { get; set; }
    public int? CollageId { get; set; }
    public int? FundId { get; set; }
    public int? DailyId { get; set; }
    public string? AuditorName { get; set; }
    public string? Details { get; set; }
    public string? CollageName { get; set; }
    public string? FundName { get; set; }
    public int? AccountId { get; set; }
    public string? AccountName { get; set; }
    //   public string? AccountNumber { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public string AccountType { get; set; } = string.Empty;


    public DateOnly? StartFrom { get; set; }
    public DateOnly? EndTo { get; set; }

}
 * 
 */
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