
using Core.Models;
namespace Shared.DTOs;




public class DailyDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly? DailyDate { get; set; }

    public string DailyType { get; set; } = string.Empty;
    public string AccountItem { get; set; }
    public decimal? TotalCredit { get; set; }
    public decimal? TotalDebit { get; set; }

    public DailyDto(Core.Models.Daily daily)
    {
        Id = daily.Id;
        Name = daily.Name ?? string.Empty;
        DailyDate = daily.DailyDate;
        DailyType = daily.DailyType;
        AccountItem = daily.AccountItem ?? string.Empty;
        TotalCredit = daily.Forms != null ? daily.Forms.Sum(x => x.FormDetails.Sum(x => x.Credit)) : 0;
        TotalDebit = daily.Forms != null ? daily.Forms.Sum(x => x.FormDetails.Sum(x => x.Debit)) : 0;


    }

    public Daily ToCore()
    {
        return new Daily
        {
            Id = Id,
            Name = Name,
            DailyDate = DailyDate!.Value,
            DailyType = DailyType
        };
    }


}
