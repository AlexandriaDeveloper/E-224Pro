using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Specification;
using Shared.Contracts.FormDetailsRequest;
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
            throw new NotImplementedException();
        }
        private async Task<List<string>> BuildHeader2(List<Form> forms)
        {
            var creditsAccount = forms
            .Select(x => x.FormDetails
            .Where(t => t.Credit.HasValue && t.Credit.Value > 0)
            .Select(y => new { AccountId = y.AccountId, AccountName = y.Account.AccountName })
            .GroupBy(x => x.AccountId)
            );
            var debitsAccount = forms
            .Select(x => x.FormDetails
            .Where(t => t.Debit.HasValue && t.Debit.Value > 0)
            .Select(y => new { AccountId = y.AccountId, AccountName = y.Account.AccountName })
            .GroupBy(x => x.AccountId)
            );
            var header = new List<string>();


            foreach (var credit in creditsAccount)
            {
                foreach (var item in credit)
                {
                    header.Add(item.AccountName);
                }
            }

            // foreach (var debit in debitsAccount)
            // {
            //     foreach (var item in debit)
            //     {
            //         header.Add(item.AccountName);
            //     }
            // }
            return header;

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

            BuildHeader2(forms);
            return formsToExcel;
        }


    }
}