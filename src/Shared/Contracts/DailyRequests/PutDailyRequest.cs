using Shared.Constants.Enums;

namespace Shared.Contracts;

public class PutDailyRequest
{
    public string? Name { get; set; }
    public DateOnly? DailyDate { get; set; }
    public string? AccountItem { get; set; }
    public string? DailyType { get; set; }

}
