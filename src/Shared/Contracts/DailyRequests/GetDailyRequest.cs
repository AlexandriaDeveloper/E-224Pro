namespace Shared.Contracts;

public class GetDailyRequest : Param
{
    public string? Name { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }
    public string? DailyType { get; set; }
    public string? AccountItem { get; set; }


}