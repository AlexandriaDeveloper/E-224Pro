using System;

namespace Shared.Contracts;


public class Param
{
    public int? PageIndex { get; set; } = 0;
    public int? PageSize { get; set; } = 30;
    public string? Direction { get; set; }
    public string? SortBy { get; set; }

}

