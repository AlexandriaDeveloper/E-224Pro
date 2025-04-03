using System;
using Shared.Constants.Enums;

namespace Shared.Contracts;

public class PostDailyRequest
{
    public string? Name { get; set; }
    public DateOnly DailyDate { get; set; }
    public string? AccountItem { get; set; }
    public string? DailyType { get; set; }


}
