using System.ComponentModel;

namespace Core.Constants;

public enum EntryTypeEnum
{
    [Description("قيد عادى")]
    NormalEntry = 0,
    [Description("قيد تصحيح")]
    ReversalEntry = 1,
    [Description("قيد سداد")]
    PaymentEntry = 2,
    [Description("قيد تسوية")]
    SettlementEntry = 3,

}
