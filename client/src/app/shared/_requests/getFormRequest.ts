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
}