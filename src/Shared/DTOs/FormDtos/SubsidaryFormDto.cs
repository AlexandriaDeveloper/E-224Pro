namespace Shared.DTOs.FormDtos;

public class SubsidaryFormResponse
{
    public List<SubsidaryFormDto> SubsidaryFormDtos { get; set; }
    public int TotalCount { get; set; }
}

public class SubsidaryFormDto
{
    public int Id { get; set; }
    public string FormName { get; set; } = string.Empty;
    public string Num224 { get; set; } = string.Empty;
    public string Num55 { get; set; } = string.Empty;
    public string FundName { get; set; } = string.Empty;
    public int FundId { get; set; }
    public int CollageId { get; set; }
    public int DailyId { get; set; }
    public string? AuditorName { get; set; }
    public string? Details { get; set; }
    public decimal? TotalCredit { get; set; }
    public decimal? TotalDebit { get; set; }
    public decimal? SubsidaryTotalCredit { get; set; }
    public decimal? SubsidaryTotalDebit { get; set; }
    public int FormDetailsId { get; set; }
    public string? CollageName { get; set; }
    public string? DailyType { get; set; }
}

public class SubsidaryFormDetailsDto
{

    public int Id { get; set; }

    public int SubAccountId { get; set; }
    public string? SubAccountNumber { get; set; }
    public string? SubAccountName { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }

}

public class AddOrUpdateSubsidaryFormDetailsRequest
{
    public int FormDetailsId { get; set; }
    public List<SubsidaryFormDetailsDto> SubsidaryFormDetailsDtos { get; set; } = new List<SubsidaryFormDetailsDto>();
}

public class GetSubsidartDailyRequest
{
    public int? FundId { get; set; }
    public int? CollageId { get; set; }
    public int? DailyId { get; set; }
    public int? AccountId { get; set; }
    public string? DailyType { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? AccountType { get; set; }

}

public class SubsidaryDailyReportDto
{
    public string? CollageName { get; set; } = "الكل";
    public string? FundName { get; set; } = "الكل";
    public string? Daily { get; set; } = "الكل";
    public string AccountType { get; set; } = "الكل";
    public string AccountName { get; set; } = "الكل";
    public List<SubsidaryDailyCollageReportDto> Collages { get; set; }

    public List<SubsidaryDailyDetailsReportDto> TotalSubsidaries { get; set; }

}
public class SubsidaryDailyCollageReportDto
{
    public int? CollageId { get; set; }
    public string CollageName { get; set; }
    public List<SubsidaryDailyFundsReportDto> Funds { get; set; }
    public decimal? Credit => Funds.Sum(x => x.Credit);
    public decimal? Debit => Funds.Sum(x => x.Debit);

}


public class SubsidaryDailyFundsReportDto
{
    public int AccountId { get; set; }
    public string AccountName { get; set; }
    public int FundId { get; set; }
    public string FundName { get; set; }
    public decimal? Credit => SubsidaryDetails.Sum(x => x.Credit);
    public decimal? Debit => SubsidaryDetails.Sum(x => x.Debit);
    public List<SubsidaryDailyDetailsReportDto> SubsidaryDetails { get; set; }


}
public class SubsidaryDailyDetailsReportDto
{
    public int SubsidaryId { get; set; }
    public string SubsidaryName { get; set; }

    public decimal? Credit { get; set; }
    public decimal? Debit { get; set; }
}

