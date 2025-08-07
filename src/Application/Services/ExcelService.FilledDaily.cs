using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Specification;
using Shared.Contracts.FormDetailsRequest;
using Shared.DTOs.Excel;
using Shared.DTOs.FormDtos;

namespace Application.Services
{
    public partial class ExcelService
    {

        public async Task<byte[]> GenerateExcelSheet(DailyFormsToExcelRequest request, CancellationToken cancellationToken = default)
        {
            List<FormsToExcelDto> data = await LoadFormData2(request, cancellationToken);
            return GenerateExcel(data);
        }

        private byte[] GenerateExcel(List<FormsToExcelDto> data)
        {
            return null;
        }
        private async Task<List<ExcelHeaderDto>> BuildFilledExcelHeader(List<Form> forms)
        {
            var creditsAccount = forms
            .SelectMany(x => x.FormDetails
            .Where(t => t.Credit.HasValue && t.Credit.Value > 0)
            .Select(y => new { AccountId = y.AccountId, AccountName = y.Account.AccountName })
            ).GroupBy(x => x.AccountId).Select(x => x.FirstOrDefault()).ToList();
            var debitsAccount = forms
            .SelectMany(x => x.FormDetails
            .Where(t => t.Debit.HasValue && t.Debit.Value > 0)
            .Select(y => new { AccountId = y.AccountId, AccountName = y.Account.AccountName })
            ).GroupBy(x => x.AccountId).Select(x => x.FirstOrDefault()).ToList();
            var excelFile = new ExcelDto();
            excelFile.Headers.Add(new ExcelHeaderDto { CodeRow = string.Empty, NameRow = "م" });
            excelFile.Headers.Add(new ExcelHeaderDto { CodeRow = "A-1", NameRow = "المراجع" });
            excelFile.Headers.Add(new ExcelHeaderDto { CodeRow = "A-2", NameRow = "رقم 55" });
            excelFile.Headers.Add(new ExcelHeaderDto { CodeRow = "A-3", NameRow = "رقم 224" });
            excelFile.Headers.Add(new ExcelHeaderDto { CodeRow = "A-4", NameRow = " الملف" });
            excelFile.Headers.Add(new ExcelHeaderDto { CodeRow = "A-5", NameRow = " الكلية" });
            excelFile.Headers.Add(new ExcelHeaderDto { CodeRow = "A-6", NameRow = " الصندوق" });
            excelFile.Headers.Add(new ExcelHeaderDto { CodeRow = "A-7", NameRow = " تفاصيل" });

            var debitAccounts = debitsAccount
            .Select(x => new ExcelHeaderDto { CodeRow = x.AccountId.ToString(), NameRow = x.AccountName })
            .ToList();


            var creditAccountNames = creditsAccount
            .Select(x => new ExcelHeaderDto { CodeRow = x.AccountId.ToString(), NameRow = x.AccountName })
            .ToList();

            excelFile.Headers.AddRange(debitAccounts);

            excelFile.Headers.Add(new ExcelHeaderDto { CodeRow = string.Empty, NameRow = "اجمالى مدين" });

            excelFile.Headers.AddRange(creditAccountNames);
            excelFile.Headers.Add(new ExcelHeaderDto { CodeRow = string.Empty, NameRow = "اجمالى دائن" });

            return excelFile.Headers;

        }

        private async Task<List<FormsToExcelDto>> LoadFormData2(DailyFormsToExcelRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new GetDailyFormToExportAsExcelSpecification(request);
            var forms = await _formRepository.GetQueryable(spec, cancellationToken).Include(x => x.FormDetails)
            .ThenInclude(x => x.Account)
            .ToListAsync(cancellationToken);
            List<FormsToExcelDto> formsToExcel = new List<FormsToExcelDto>();
            foreach (var form in forms)
            {
                formsToExcel.Add(new FormsToExcelDto
                {
                    Id = form.Id,
                    FormName = form.FormName,
                    CollageName = form.Collage.CollageName,
                    FundName = form.Fund.FundName,
                    AuditorName = form.AuditorName,
                    Num224 = form.Num224,
                    Num55 = form.Num55,
                    Credit = form.FormDetails.Select(x => new ItemAmount()
                    {
                        AccountId = x.AccountId,
                        Amount = x.Credit.HasValue ? x.Credit.Value : 0,

                    }).ToList(),
                    Debit = form.FormDetails.Select(x => new ItemAmount()
                    {
                        AccountId = x.AccountId,
                        Amount = x.Debit.HasValue ? x.Debit.Value : 0,

                    }).ToList(),
                    TotalCredit = form.TotalCredit,
                    TotalDebit = form.TotalDebit

                });
            }
            //Step 1  : Build Excel Header
            await BuildFilledExcelHeader(forms);
            //Step 2  : Build Excel Data
            await BuildFilledExcelData(formsToExcel);

            return formsToExcel;
        }

        private async Task BuildFilledExcelData(List<FormsToExcelDto> formsToExcel)
        {
            foreach (var form in formsToExcel)
            {
                var row = new ExcelRowDto();
                row.Cells.Add(new ExcelCellDto { CodeRow = string.Empty, Value = form.Id.ToString() });
                row.Cells.Add(new ExcelCellDto { CodeRow = "A-1", Value = form.FormName ?? string.Empty });
                row.Cells.Add(new ExcelCellDto { CodeRow = "A-2", Value = form.Num55 ?? string.Empty });
                row.Cells.Add(new ExcelCellDto { CodeRow = "A-3", Value = form.Num224 ?? string.Empty });
                row.Cells.Add(new ExcelCellDto { CodeRow = "A-4", Value = form.AuditorName ?? string.Empty });
                row.Cells.Add(new ExcelCellDto { CodeRow = "A-5", Value = form.CollageName ?? string.Empty });
                row.Cells.Add(new ExcelCellDto { CodeRow = "A-6", Value = form.FundName ?? string.Empty });
                row.Cells.Add(new ExcelCellDto { CodeRow = "A-7", Value = form.FormName ?? string.Empty });
                foreach (var item in form.Debit)
                {
                    row.Cells.Add(new ExcelCellDto { CodeRow = item.AccountId.ToString(), Value = item.Amount.ToString() ?? string.Empty });
                }
                row.Cells.Add(new ExcelCellDto { CodeRow = string.Empty, Value = form.TotalDebit.ToString() ?? string.Empty });
                foreach (var item in form.Credit)
                {
                    row.Cells.Add(new ExcelCellDto { CodeRow = item.AccountId.ToString(), Value = item.Amount.ToString() ?? string.Empty });
                }
                row.Cells.Add(new ExcelCellDto { CodeRow = string.Empty, Value = form.TotalCredit.ToString() ?? string.Empty });
                row.Cells.Add(new ExcelCellDto { CodeRow = string.Empty, Value = (form.TotalCredit - form.TotalDebit).ToString() ?? string.Empty });
                if (form.TotalCredit != 0)
                {
                    // row.Cells.Add(new ExcelCellDto { CodeRow = string.Empty, Value = ((form.TotalCredit - form.TotalDebit) / form.TotalCredit) * 100.ToString() }); 
                }
            }
        }
    }
}