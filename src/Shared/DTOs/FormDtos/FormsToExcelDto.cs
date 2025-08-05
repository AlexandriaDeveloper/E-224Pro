using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOs.FormDtos
{
    public class FormsToExcelDto
    {
        public int Id { get; set; }
        public string? AuditorName { get; set; }
        public string? Num55 { get; set; }
        public string? Num224 { get; set; }

        public string? FormName { get; set; }
        public string? FundName { get; set; }
        public string? CollageName { get; set; }
        public string? Details { get; set; }
        public List<ItemAmount> Debit { get; set; } = new List<ItemAmount>();
        public decimal? TotalDebit { get; set; }
        public List<ItemAmount> Credit { get; set; } = new List<ItemAmount>();
        public decimal? TotalCredit { get; set; }
        public decimal Net { get; set; }



    }
    public class ItemAmount
    {
        public int AccountId { get; set; }
        public decimal? Amount { get; set; }
    }
    public class DailyFormsToExcelRequest
    {
        public int? DailyId { get; set; }
        public int? FormId { get; set; }
        public int? AuditorName { get; set; }
        public int? CollageId { get; set; }
        public int? FundId { get; set; }
        public int? EntryType { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? DailyType { get; set; }

    }
}