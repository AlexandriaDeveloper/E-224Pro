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
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public partial class ExcelService
    {
        private readonly ICollageRepository _collageRepository;
        private readonly SubSidaryDailyService _subsidaryDailyService;
        private readonly ISubsidiaryJournalRepository _subsidaryJournalRepository;

        private readonly IFundRepository _fundRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IDailyRepository _dailyRepository;
        private readonly IUow _uow;

        public ExcelService(IDailyRepository dailyRepository, IUow uow, ISubsidiaryJournalRepository subsidiaryJournalRepository, ICollageRepository collageRepository, SubSidaryDailyService subsidaryDailyService, IFundRepository fundRepository, IAccountRepository accountRepository)
        {
            _collageRepository = collageRepository;
            _subsidaryDailyService = subsidaryDailyService;
            _fundRepository = fundRepository;
            _accountRepository = accountRepository;
            _dailyRepository = dailyRepository;
            _uow = uow;
            _subsidaryJournalRepository = subsidiaryJournalRepository;


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

        public async Task UploadSubsidiaryExcelFile(int dailyId, int accountId, IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentNullException(nameof(file));
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var tempPath = Path.GetTempPath();
            if (tempPath == null)
            {

                throw new Exception("Error getting temp path");
            }
            var filePath = Path.Combine(tempPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var excelService = new NpoiService(filePath); // Assuming you have a method to create an instance of NpoiService
            var sheetHeader = excelService.GetSheetHeader(0, "Subsidiary Data");
            var sheetData = excelService.GetSheetData(2, "Subsidiary Data");

            // Find indices of headers that are numbers only (no letters, not empty)
            var numericHeaderIndices = new List<int>();
            for (int i = 0; i < sheetHeader.Count(); i++)
            {
                var cell = sheetHeader[i]?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(cell) && cell.All(char.IsDigit))
                {
                    numericHeaderIndices.Add(i);
                }
            }

            // For debugging: print the indices
            Console.WriteLine("Numeric header indices: " + string.Join(", ", numericHeaderIndices));
            var subsToAdd = new List<SubsidiaryJournal>();
            // Example: Loop through each row in sheetData
            foreach (var row in sheetData)
            {
                // For each numeric header index, get the value (assume it's formDetailsId or similar)
                var rowList = row as IList<object>;
                if (rowList == null) continue;
                foreach (var colIdx in numericHeaderIndices)
                {
                    var cellValue = rowList[colIdx]?.ToString()?.Trim();
                    if (string.IsNullOrEmpty(cellValue)) continue;
                    decimal cellValueParsed;
                    if (!decimal.TryParse(cellValue, out cellValueParsed)) continue;

                    // Check if record exists in Subsidiary table by formDetailsId
                    var formDetailsIdValue = rowList[0]?.ToString();
                    var subAccountIdValue = sheetHeader[colIdx]?.ToString();
                    int parsedFormDetailsId = 0;
                    int parsedSubAccountId = 0;
                    if (!int.TryParse(formDetailsIdValue, out parsedFormDetailsId) || !int.TryParse(subAccountIdValue, out parsedSubAccountId))
                        continue;

                    var existing = _subsidaryJournalRepository.GetQueryable()
                        .Where(x => x.FormDetailsId == parsedFormDetailsId && x.SubAccountId == parsedSubAccountId)
                        .AsNoTracking()
                        .FirstOrDefault(); // You must implement this method


                    if (existing != null)
                    {
                        // // Update the record
                        if (int.Parse(cellValue) > 0)
                        {
                            existing.Credit = decimal.Parse(cellValue);
                        }
                        else
                        {
                            existing.Debit = decimal.Parse(cellValue) * -1;
                        }


                        await _subsidaryJournalRepository.UpdateAsync(existing); // You must implement this method
                    }
                    else
                    {
                        // Create a new record
                        var subsidiaryJournal = new SubsidiaryJournal
                        {
                            // Id = _subsidaryJournalRepository.GetQueryable().Max(x => x.Id) + 1,
                            FormDetailsId = parsedFormDetailsId,
                            SubAccountId = int.Parse(sheetHeader[colIdx]),
                            Credit = decimal.Parse(cellValue) > 0 ? decimal.Parse(cellValue) : 0,
                            Debit = decimal.Parse(cellValue) < 0 ? decimal.Parse(cellValue) * -1 : 0
                        };
                        subsToAdd.Add(subsidiaryJournal);
                    }

                }

            }
            await _subsidaryJournalRepository.AddRange2Async(subsToAdd);
            await _uow.CommitAsync(cancellationToken);

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

