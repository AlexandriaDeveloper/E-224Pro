using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel.Charts;
using NPOI.SS.Util;
using System.Collections.Generic;
using System.IO;
using Core.Interfaces.Repository;
using Core.Models;
using System.Linq;

using Shared.Contracts.FormDetailsRequest;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Helpers;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.AccountDtos;

using Shared.DTOs.FormDtos;

namespace Application.Services
{
    public partial class ExcelService
    {
        private readonly ICollageRepository _collageRepository;
        private readonly SubSidaryDailyService _subsidaryDailyService;
        private readonly IFundRepository _fundRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IDailyRepository _dailyRepository;

        public ExcelService(IDailyRepository dailyRepository, ICollageRepository collageRepository, SubSidaryDailyService subsidaryDailyService, IFundRepository fundRepository, IAccountRepository accountRepository)
        {
            _collageRepository = collageRepository;
            _subsidaryDailyService = subsidaryDailyService;
            _fundRepository = fundRepository;
            _accountRepository = accountRepository;
            _dailyRepository = dailyRepository;

        }


        // Helper function to convert 0-based column index to Excel column name (A, B, ..., AA, AB, ...)
        private string GetExcelColumnName(int columnIndex)
        {
            // columnIndex is 0-based
            int dividend = columnIndex + 1; // Convert to 1-based for calculation
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / 26;
            }
            return columnName;
        }



        // New method: Generate and fill Excel template with data by dailyId
        public async Task<byte[]> GenerateFilledExcelSheetByDailyId(int dailyId, CancellationToken cancellationToken)
        {
            // Load the daily and related data (adjust repository and includes as needed)
            // Example assumes you have a DailyRepository and FormRepository available
            // You may need to inject them into this service if not already
            var daily = _dailyRepository.GetQueryable(null)
            .Where(d => d.Id == dailyId)
            .Include(x => x.Forms)
            .Include(d => d.Forms).ThenInclude(f => f.FormDetails)
            .ThenInclude(f => f.Account).FirstOrDefault(); // Replace with your actual repository method
            if (daily == null)
                throw new Exception($"No daily found with id {dailyId}");
            // Get Distanicat accounts for Debit and Credit


            var headers = BuildHeader(daily.Forms);
            //  Console.WriteLine($"Debit Accounts Count: {debitAccounts.Count}, Credit Accounts Count: {creditAccounts.Count}");

            throw new NotImplementedException(); // Placeholder for the actual implementation
        }
        private ExcelHeaders BuildHeader(List<Form> forms)
        {
            var debitAccounts = forms
                    .SelectMany(f => f.FormDetails)
                    .Where(fd => fd.Account != null && fd.AccountId.ToString().StartsWith("1")).GroupBy(x => x.AccountId) // Assuming Debit accounts start with "1"
                    .Select(fd => new AccountDto
                    {
                        AccountName = fd.First().Account.AccountName!,
                        Id = fd.Key
                    })

                    .ToList();
            var creditAccounts = forms
                .SelectMany(f => f.FormDetails)
                .Where(fd => fd.Account != null && fd.AccountId.ToString().StartsWith("2")).GroupBy(x => x.AccountId) // Assuming Credit accounts start with "2"
                .Select(fd => new AccountDto
                {
                    AccountName = fd.First().Account.AccountName!,
                    Id = fd.Key
                })
                .ToList();
            // Build the headers using the ExcelHeaders class
            var headers = new ExcelHeaders().Build(debitAccounts, creditAccounts);
            return headers;
        }



        public class ExcelHeaders
        {

            public List<ExcelHeadrsTitles> Headers { get; set; } = new List<ExcelHeadrsTitles>();

            public ExcelHeaders()
            {

            }
            public ExcelHeaders Build(List<AccountDto> debitAccounts, List<AccountDto> creditAccounts)
            {
                Headers.Clear();
                Headers.Add(new ExcelHeadrsTitles { row1 = string.Empty, row2 = "م" });

                Headers.Add(new ExcelHeadrsTitles { row1 = "A-1", row2 = "رقم 55" });
                Headers.Add(new ExcelHeadrsTitles { row1 = "A-2", row2 = "رقم 224" });
                Headers.Add(new ExcelHeadrsTitles { row1 = "A-3", row2 = "أسم الملف" });
                Headers.Add(new ExcelHeadrsTitles { row1 = "A-4", row2 = "المراجع" });
                Headers.Add(new ExcelHeadrsTitles { row1 = "A-5", row2 = "الكلية" });
                Headers.Add(new ExcelHeadrsTitles { row1 = "A-6", row2 = "الصندوق" });
                Headers.Add(new ExcelHeadrsTitles { row1 = "A-7", row2 = "تفاصيل" });
                foreach (var account in debitAccounts)
                {
                    Headers.Add(new ExcelHeadrsTitles { row1 = account.Id.ToString(), row2 = account.AccountName });
                }
                Headers.Add(new ExcelHeadrsTitles { row1 = "TotalDebit", row2 = "اجمالى مدين" });
                foreach (var account in creditAccounts)
                {
                    Headers.Add(new ExcelHeadrsTitles { row1 = account.Id.ToString(), row2 = account.AccountName });
                }
                Headers.Add(new ExcelHeadrsTitles { row1 = "TotalCredit", row2 = "اجمالى دائن" });
                Headers.Add(new ExcelHeadrsTitles { row1 = "Net", row2 = "الصافى" });

                return this;
            }
        }
        public class ExcelHeadrsTitles
        {
            public string row1 { get; set; } = string.Empty;
            public string row2 { get; set; } = string.Empty;

        }







    }
}

