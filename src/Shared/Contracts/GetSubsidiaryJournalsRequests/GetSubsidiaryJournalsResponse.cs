public class GetSubsidiaryJournalResponse
{

    public decimal? TotalCredit => subsidiaryJournalDtos.Sum(x => x.Credit);
    public decimal? TotalDebit => subsidiaryJournalDtos.Sum(x => x.Debit);

    public decimal? Balance => TotalCredit.HasValue && TotalDebit.HasValue ? TotalCredit - TotalDebit : null;
    public int count => subsidiaryJournalDtos.Count();
    public List<SubsidiaryJournalDto> subsidiaryJournalDtos { get; set; }
}