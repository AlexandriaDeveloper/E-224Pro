using System;
using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace Core.Models;

public class Daily : BaseEntity
{

    public DateOnly DailyDate { get; set; }
    //payroll or general
    [MaxLength(50)]
    public string DailyType { get; set; } = string.Empty;
    //

    //موزازنه ام صناديق
    //Budgetary Expenses
    // Special Fund Expensess
    // [MaxLength(20)]
    // public string? AccountItem { get; set; }
    public List<Form>? Forms { get; set; }

}
