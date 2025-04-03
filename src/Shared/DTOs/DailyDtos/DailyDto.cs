
using Core.Models;
namespace Shared.DTOs;



public class DailiesResponse
{
    public List<DailyDto> Dailies { get; set; } = new List<DailyDto>();
    public int TotalCount { get; set; }
}
public class DailyDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly? DailyDate { get; set; }

    public string DailyType { get; set; } = string.Empty;
    public string AccountItem { get; set; }

    public DailyDto(Core.Models.Daily daily)
    {
        Id = daily.Id;
        Name = daily.Name ?? string.Empty;
        DailyDate = daily.DailyDate;
        DailyType = daily.DailyType;
        AccountItem = daily.AccountItem ?? string.Empty;


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
