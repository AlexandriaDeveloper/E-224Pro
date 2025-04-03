using System.ComponentModel;

namespace Shared.Constants.Enums;

public enum AccountItemEnum
{
    //i need name with spaces
    [Description("موازنه")]
    StateBudget = 0,
    [Description("صناديق خاصة")]
    SpecialFunds = 1,

}

public enum TransactionSideEnum
{
    Debit = 0,
    Credit = 1

}
