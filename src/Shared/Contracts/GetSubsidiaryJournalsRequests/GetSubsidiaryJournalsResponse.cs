public class GetSubsidiaryJournalResponse
{

    public decimal? TotalCredit => subsidiaryJournalDtos.Where(x => x.TransactionSide == "Credit").Sum(x => x.Amount);
    public decimal? TotalDebit => subsidiaryJournalDtos.Where(x => x.TransactionSide == "Debit").Sum(x => x.Amount);

    public decimal? Balance => TotalCredit.HasValue && TotalDebit.HasValue ? TotalCredit - TotalDebit : null;
    public int count => subsidiaryJournalDtos.Count();
    public List<SubsidiaryJournalDto> subsidiaryJournalDtos { get; set; }
}