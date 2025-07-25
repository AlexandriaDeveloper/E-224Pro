using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.Formula.Eval;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using Persistence.Specification;
using Shared.Common;
using Shared.Contracts;
using Shared.DTOs;
using Shared.DTOs.FormDetailsDtos;
using Shared.DTOs.FormDtos;
using Shared.DTOs.ReportDtos;
using static Shared.DTOs.FormDtos.SubsidaryToExcelDto; // Add this if SubsidaryDailyFormDetailsDto is in ReportDtos namespace

public class SubSidaryDailyService
{
    private readonly IDailyRepository _dailyRepository;
    private readonly IFormRepository _formRepository;
    private readonly IFormDetailsRepository _formDetailsRepository;
    private readonly ISubAccountRepository _subAccountRepository;
    private readonly ICollageRepository _collageRepository;
    private readonly IFundRepository _fundRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ISubsidiaryJournalRepository _subsidiaryJournalRepository;
    private readonly IUow _uow;
    public SubSidaryDailyService(IDailyRepository dailyRepository, IAccountRepository accountRepository, IFormRepository formRepository, IFundRepository fundRepository, IFormDetailsRepository formDetailsRepository, ISubAccountRepository subAccountRepository, ISubsidiaryJournalRepository subsidiaryJournalRepository, IUow uow, ICollageRepository collageRepository)
    {
        _dailyRepository = dailyRepository;
        this._formRepository = formRepository;
        this._formDetailsRepository = formDetailsRepository;
        this._subAccountRepository = subAccountRepository;
        this._subsidiaryJournalRepository = subsidiaryJournalRepository;
        this._collageRepository = collageRepository;
        this._fundRepository = fundRepository;
        this._accountRepository = accountRepository;

        _uow = uow;
    }

    public async Task<PaginatedResult<DailyDto>> GetDailiesBySpec(GetDailyRequest request, CancellationToken cancellationToken = default)
    {
        var spec = new DailySpecification(request);

        var dailies = await _dailyRepository.GetAll(spec, cancellationToken);
        var dailyCountSpec = new DailyCountAsyncSpecification(request);
        var dailyCountResult = await _dailyRepository.CountAsync(dailyCountSpec);

        // Return the daily
        // DailiesResponse dailiesResponse = new DailiesResponse(); //
        var dailiesResponse = dailies.Select(x => new DailyDto(x!, false)).ToList();


        return PaginatedResult<DailyDto>.Create(dailiesResponse, request.PageIndex, request.PageSize, dailyCountResult);

    }
    public async Task<PaginatedResult<DailyDto>> GetSubsidaryDailiesBySpec(int accountId, GetDailyRequest request, CancellationToken cancellationToken = default)
    {
        var spec = new DailySpecification(request);

        var dailies = _dailyRepository.GetQueryable(spec).Include(x => x.Forms!).ThenInclude(x => x.FormDetails.Where(x => x.AccountId == accountId)).ThenInclude(x => x.SubsidiaryJournals);
        var dailyCountSpec = new DailyCountAsyncSpecification(request);
        var dailyCountResult = await _dailyRepository.CountAsync(dailyCountSpec);

        // Return the daily

        var dailiesResponse = dailies.Select(x => new DailyDto(x!, false)).ToList();


        return PaginatedResult<DailyDto>.Create(dailiesResponse, request.PageIndex, request.PageSize, dailyCountResult);

    }
    public async Task<PaginatedResult<SubsidaryFormDto>> GetSubsidaryDailyFormsByDailyIdAndSubsidaryId(int subaccountId, int dailyId, GetSubsidiaryFormsByDailyIdRequest request, CancellationToken cancellationToken = default)
    {

        var spec = new GetSubsidaryFormsSpecification(request);


        var subsidaryDailyForms = await _formRepository.GetQueryable(spec)
        .Include(x => x.Collage)
        .Include(x => x.Fund)
        .Include(x => x.Daily)

        .Include(x => x.FormDetails)
        .ThenInclude(x => x.SubsidiaryJournals)
         .Where(x => x.DailyId == dailyId && x.FormDetails.Any(x => x.AccountId == subaccountId))
        .ToListAsync(cancellationToken); // Use ToListAsync to execute the query and get the results
        var specCount = new GetSubsidaryFormsCountSpecification(request);
        var subsidaryDailyFormsCount = await _formRepository.CountAsync(specCount, cancellationToken);

        var subsidiaryForms = subsidaryDailyForms.Select(x =>
           {

               return new SubsidaryFormDto()
               {
                   Id = x.Id,
                   FormName = x.FormName,
                   TotalCredit = x.FormDetails.Where(x => x.AccountId == subaccountId).Sum(x => x.Credit),
                   TotalDebit = x.FormDetails.Where(x => x.AccountId == subaccountId).Sum(x => x.Debit),
                   SubsidaryTotalCredit = x.FormDetails.Where(x => x.AccountId == subaccountId).Sum(x => x.SubsidiaryJournals?.Sum(x => x.Credit) ?? 0),
                   SubsidaryTotalDebit = x.FormDetails.Where(x => x.AccountId == subaccountId).Sum(x => x.SubsidiaryJournals?.Sum(x => x.Debit) ?? 0),
                   FormDetailsId = x.FormDetails.Where(x => x.AccountId == subaccountId).FirstOrDefault()!.Id,
                   CollageId = x.CollageId ?? 0,
                   CollageName = x.Collage?.CollageName ?? string.Empty,
                   FundId = x.FundId ?? 0,
                   FundName = x.Fund?.FundName ?? string.Empty,
                   Num224 = x.Num224 ?? string.Empty,
                   Num55 = x.Num55 ?? string.Empty,

                   DailyId = x.DailyId,
                   AuditorName = x.AuditorName,
                   Details = x.Details,
               };
           }).ToList();


        return PaginatedResult<SubsidaryFormDto>.Create(subsidiaryForms, request.PageIndex, request.PageSize, subsidaryDailyFormsCount);
    }

    public async Task<List<SubsidaryFormDetailsDto>> GetSubsidaryFormDetailsByFormDetailsId(int formDetailsId, int subaccountId, CancellationToken cancellationToken = default)
    {
        List<SubAccount> subs = _subAccountRepository.GetQueryable(null)
                .Where(x => x.AccountId == subaccountId)
                .AsNoTracking().ToList();
        // create 
        List<SubsidaryFormDetailsDto> subSidaryFormDetails = new List<SubsidaryFormDetailsDto>();

        foreach (var sub in subs)
        {
            //if subsidary has record with account id add it to subSidaryFormDetails else  add new subsidary form details with zero credit and debit
            var subsidaryFormDetails = await _subsidiaryJournalRepository.GetQueryable(null)
            .Include(x => x.SubAccount)
                .Where(x => x.FormDetailsId == formDetailsId && x.SubAccountId == sub.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (subsidaryFormDetails != null)
            {
                subSidaryFormDetails.Add(new SubsidaryFormDetailsDto()
                {
                    Id = subsidaryFormDetails.Id,
                    SubAccountId = subsidaryFormDetails.SubAccountId,
                    SubAccountName = subsidaryFormDetails.SubAccount?.SubAccountName ?? string.Empty,
                    SubAccountNumber = subsidaryFormDetails.SubAccount?.Id ?? 0,

                    Credit = subsidaryFormDetails.Credit,
                    Debit = subsidaryFormDetails.Debit
                });
            }
            else
            {
                subSidaryFormDetails.Add(new SubsidaryFormDetailsDto()
                {
                    SubAccountId = sub.Id,
                    SubAccountName = sub.SubAccountName ?? string.Empty,
                    SubAccountNumber = sub.Id,
                    Credit = 0,
                    Debit = 0

                });
            }




        }

        return subSidaryFormDetails;
    }

    public async Task<bool> AddOrUpdateSubsidaryFormDetail(AddOrUpdateSubsidaryFormDetailsRequest dto, CancellationToken cancellationToken = default)

    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        // Get all existing entities for this FormDetailsId in one query
        var existingEntities = await _subsidiaryJournalRepository.GetQueryable(null)
            .Where(x => x.FormDetailsId == dto.FormDetailsId)
            .ToListAsync(cancellationToken);

        // get maxId 
        var maxId = await _subsidiaryJournalRepository.GetQueryable(null)
           .MaxAsync(x => (int?)x.Id) ?? 0;

        // Process each subsidiary form detail in the request
        foreach (var detailDto in dto.SubsidaryFormDetailsDtos)
        {
            // Find existing entity in the already-loaded list
            var entity = existingEntities.FirstOrDefault(x => x.SubAccountId == detailDto.SubAccountId);

            if (entity != null)
            {
                // Update existing entity
                entity.Credit = detailDto.Credit;
                entity.Debit = detailDto.Debit;
            }
            else
            {
                // Create new only if Credit or Debit has value > 0
                if ((detailDto.Credit.HasValue && detailDto.Credit > 0) ||
                    (detailDto.Debit.HasValue && detailDto.Debit > 0))
                {
                    var newEntity = new SubsidiaryJournal
                    {
                        Id = ++maxId, // Increment maxId for new entity
                        FormDetailsId = dto.FormDetailsId,
                        SubAccountId = detailDto.SubAccountId,
                        Credit = detailDto.Credit,
                        Debit = detailDto.Debit,
                    };
                    await _subsidiaryJournalRepository.AddAsync(newEntity, cancellationToken);
                }
                if (detailDto.Credit == 0 && detailDto.Debit == 0)
                {
                    // If both Credit and Debit are zero, remove the entity if it exists
                    var entityToRemove = existingEntities.FirstOrDefault(x => x.Id == detailDto.Id);
                    if (entityToRemove != null)
                    {
                        _subsidiaryJournalRepository.Delete(entityToRemove, cancellationToken);
                    }
                }
            }
        }
        await _uow.CommitAsync(cancellationToken);
        return true;
    }
    public async Task<SubsidaryDailyReportDto> GetSubsidaryDaily(GetSubsidartDailyRequest request)
    {
        var (collages, funds, accounts) = await LoadRequiredDataAsync();
        var spec = new GetSubsidaryDailyBySpecification(request);
        var subs = _formDetailsRepository.GetQueryable(spec).ToList();

        if (!subs.Any())
        {
            return BuildEmptyReportDto(request, collages, funds, accounts);
        }

        var subsResult = BuildSubsidiaryCollageReport(subs, collages, funds);
        var totalSubsidaries = CalculateTotalSubsidaries(subsResult);

        return BuildFinalReportDto(request, collages, funds, accounts, subs, subsResult, totalSubsidaries);
    }

    private async Task<(IEnumerable<Collage> collages, IEnumerable<Fund> funds, IEnumerable<Account> accounts)> LoadRequiredDataAsync()
    {
        var collages = (await _collageRepository.GetAll(null)).Where(c => c != null).Cast<Collage>().ToList();
        var funds = (await _fundRepository.GetAll(null)).Where(f => f != null).Cast<Fund>().ToList();
        var accounts = (await _accountRepository.GetAll(null)).Where(a => a != null).Cast<Account>().ToList();
        return (collages, funds, accounts);
    }

    public async Task<List<SubsidaryToExcelDto>> BuildSubsidaryToExcelData(SubsidaryToExcelRequest request, CancellationToken cancellationToken = default)
    {
        var spec = new SubsidaryToExcelSpecification(request);
        var data = _formDetailsRepository.GetQueryable(spec).ToList();

        if (!request.AccountId.HasValue)
        {
            throw new Exception("AccountId is required");
        }

        // Get all subaccounts for this account, regardless of whether they have subsidiary journal entries
        var subAccounts = await _subAccountRepository.GetSubAccountsByAccountId(request.AccountId.Value, cancellationToken);

        if (!subAccounts.Any())
        {
            throw new Exception("No subaccounts found for the specified account");
        }

        var result = new List<SubsidaryToExcelDto>();

        // If no form details data, still create entries with zero values for all subaccounts
        if (!data.Any())
        {
            // Create a single entry with all subaccounts having zero values
            var emptyDto = new SubsidaryToExcelDto
            {

                CollageName = string.Empty,
                FundName = string.Empty,
                AuditorName = string.Empty,
                Details = string.Empty,
                FormName = string.Empty,
                Num224 = string.Empty,
                Num55 = string.Empty,
                TotalCredit = 0,
                TotalDebit = 0,
                SubsidaryAccountDtos = subAccounts.Select(sa => new SubsidaryToExcelDto.SubsidaryAccountDto
                {
                    Id = sa.Id,
                    AccountName = sa.SubAccountName,
                    AccountNumber = string.Empty, // SubAccountNumber was removed from model
                    Credit = 0,
                    Debit = 0
                }).ToList()
            };
            result.Add(emptyDto);
            return result;
        }

        // Process form details data with subsidiary journal entries
        foreach (var item in data)
        {
            var dto = new SubsidaryToExcelDto
            {

                CollageName = item.Form?.Collage?.CollageName ?? string.Empty,
                FundName = item.Form?.Fund?.FundName ?? string.Empty,
                AuditorName = item.Form?.AuditorName ?? string.Empty,
                Details = item.Form?.Details ?? string.Empty,
                FormName = item.Form?.FormName ?? string.Empty,
                Num224 = item.Form?.Num224 ?? string.Empty,
                Num55 = item.Form?.Num55 ?? string.Empty,
                TotalCredit = item.Form?.FormDetails.Where(x => x.AccountId == item.AccountId).Sum(x => x.Credit) ?? 0,
                TotalDebit = item.Form?.FormDetails.Where(x => x.AccountId == item.AccountId).Sum(x => x.Debit) ?? 0,
                SubsidaryAccountDtos = new List<SubsidaryToExcelDto.SubsidaryAccountDto>()
            };

            // For each subaccount, find corresponding subsidiary journal entries or add with zero values
            foreach (var subAccount in subAccounts)
            {
                // Try to find subsidiary journal entries for this subaccount in this form detail
                var subsidiaryEntries = item.SubsidiaryJournals?
                    .Where(sj => sj.SubAccountId == subAccount.Id)
                    .ToList() ?? new List<SubsidiaryJournal>();

                if (subsidiaryEntries.Any())
                {
                    // Add subaccount with actual values from subsidiary entries
                    dto.SubsidaryAccountDtos.Add(new SubsidaryToExcelDto.SubsidaryAccountDto
                    {
                        Id = subAccount.Id,
                        AccountName = subAccount.SubAccountName,
                        AccountNumber = string.Empty, // SubAccountNumber was removed from model
                        Credit = subsidiaryEntries.Sum(se => se.Credit) ?? 0,
                        Debit = subsidiaryEntries.Sum(se => se.Debit) ?? 0
                    });
                }
                else
                {
                    // Add subaccount with zero values since no subsidiary entries exist
                    dto.SubsidaryAccountDtos.Add(new SubsidaryToExcelDto.SubsidaryAccountDto
                    {
                        Id = subAccount.Id,
                        AccountName = subAccount.SubAccountName,
                        AccountNumber = string.Empty, // SubAccountNumber was removed from model
                        Credit = 0,
                        Debit = 0
                    });
                }
            }

            result.Add(dto);
        }

        return result;
    }

    /// <summary>
    /// Generates an Excel file from subsidiary data with 2 header rows
    /// </summary>
    /// <param name="request">The request containing filter parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Byte array representing the Excel file</returns>
    public async Task<byte[]> GenerateSubsidiaryExcelFile(SubsidaryToExcelRequest request, CancellationToken cancellationToken = default)
    {
        // Get the data from BuildSubsidaryToExcelData method
        var data = await BuildSubsidaryToExcelData(request, cancellationToken);
        if (data == null || !data.Any())
        {
            throw new Exception("No data found to export to Excel");
        }

        // Create a new workbook and sheet
        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet("Subsidiary Data");
        sheet.IsRightToLeft = true; // Set the sheet to be right-to-left for Arabic text
        sheet.CreateFreezePane(0, 2); // Freeze the first 2 rows (headers)

        // Helper function to get cell style for headers
        ICellStyle GetHeaderStyle()
        {
            var style = workbook.CreateCellStyle();
            var font = workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 12;
            style.SetFont(font);
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.FillForegroundColor = IndexedColors.LightYellow.Index;
            style.FillPattern = FillPattern.SolidForeground;
            return style;
        }

        // Get all unique subaccounts from the first entry (all entries should have the same set of subaccounts)
        var subAccounts = data.FirstOrDefault()?.SubsidaryAccountDtos ?? new List<SubsidaryAccountDto>();

        // Define column indices
        int fixedColumnCount = 10; // ID, FormName, CollageName, FundName, Num55, Num224, AuditorName, Details, TotalDebit,TotalCredit
        int subAccountsStartIndex = fixedColumnCount;
        int totalColumnCount = subAccountsStartIndex + subAccounts.Count + 2; // +1 for TotalCredit at the end

        var startSubAccountLetter = GetExcelColumnName(subAccountsStartIndex);
        var endSubAccountLetter = GetExcelColumnName(subAccountsStartIndex + subAccounts.Count - 1);

        // Create the first header row with ID numbers or codes
        var headerCodeRow = sheet.CreateRow(0);
        headerCodeRow.CreateCell(0).SetCellValue(string.Empty); // ID
        headerCodeRow.CreateCell(1).SetCellValue("A-1"); // Num55
        headerCodeRow.CreateCell(2).SetCellValue("A-2"); // Num224
        headerCodeRow.CreateCell(3).SetCellValue("A-3"); // Form Name
        headerCodeRow.CreateCell(4).SetCellValue("A-4"); // Auditor Name
        headerCodeRow.CreateCell(5).SetCellValue("A-5"); // College Name
        headerCodeRow.CreateCell(6).SetCellValue("A-6"); // Fund Name
        headerCodeRow.CreateCell(7).SetCellValue("A-7"); // Details
        headerCodeRow.CreateCell(8).SetCellValue(string.Empty); // Total Debit
        headerCodeRow.CreateCell(9).SetCellValue(string.Empty); // Total Credit

        // Add subaccount IDs to the first header row
        for (int i = 0; i < subAccounts.Count; i++)
        {
            headerCodeRow.CreateCell(subAccountsStartIndex + i).SetCellValue(subAccounts[i].Id); // Use index+1 as ID
        }



        // Apply header style to all cells in the first header row
        var headerStyle = GetHeaderStyle();
        for (int i = 0; i < totalColumnCount; i++)
        {
            var cell = headerCodeRow.GetCell(i) ?? headerCodeRow.CreateCell(i);
            cell.CellStyle = headerStyle;
        }

        // Create the second header row with names
        var headerNameRow = sheet.CreateRow(1);
        headerNameRow.CreateCell(0).SetCellValue("الرقم");
        headerNameRow.CreateCell(1).SetCellValue("رقم 55");
        headerNameRow.CreateCell(2).SetCellValue("رقم 224");
        headerNameRow.CreateCell(3).SetCellValue("اسم الملف");
        headerNameRow.CreateCell(4).SetCellValue("المراجع");
        headerNameRow.CreateCell(5).SetCellValue("الكلية");
        headerNameRow.CreateCell(6).SetCellValue("الصندوق");
        headerNameRow.CreateCell(7).SetCellValue("تفاصيل");
        headerNameRow.CreateCell(8).SetCellValue("إجمالي مدين");
        headerNameRow.CreateCell(9).SetCellValue("إجمالي دائن");


        // Add subaccount names to the second header row
        for (int i = 0; i < subAccounts.Count; i++)
        {
            headerNameRow.CreateCell(subAccountsStartIndex + i).SetCellValue(subAccounts[i].AccountName);
        }

        // Add Total Credit to the second header row
        headerNameRow.CreateCell(subAccountsStartIndex + subAccounts.Count).SetCellValue(" الصافى");
        headerNameRow.CreateCell(subAccountsStartIndex + subAccounts.Count + 1).SetCellValue(" التوازن");


        // Apply header style to all cells in the second header row
        for (int i = 0; i < totalColumnCount; i++)
        {
            var cell = headerNameRow.GetCell(i) ?? headerNameRow.CreateCell(i);
            cell.CellStyle = headerStyle;
        }

        // Create a style for data cells
        ICellStyle dataStyle = workbook.CreateCellStyle();
        dataStyle.BorderTop = BorderStyle.Thin;
        dataStyle.BorderBottom = BorderStyle.Thin;
        dataStyle.BorderLeft = BorderStyle.Thin;
        dataStyle.BorderRight = BorderStyle.Thin;

        // Create a style for numeric cells
        ICellStyle numericStyle = workbook.CreateCellStyle();
        numericStyle.CloneStyleFrom(dataStyle);
        numericStyle.DataFormat = workbook.CreateDataFormat().GetFormat("0.00");

        // Fill the data rows
        for (int rowIndex = 0; rowIndex < data.Count; rowIndex++)
        {
            var item = data[rowIndex];
            var row = sheet.CreateRow(rowIndex + 2); // +2 because we have 2 header rows

            // Set the fixed column data
            // row.CreateCell(0).SetCellValue(rowIndex + 1); // ID number (1-based)
            SetCellValueWithStyle(row, 0, (rowIndex + 1).ToString(), dataStyle);
            SetCellValueWithStyle(row, 1, item.Num55, dataStyle);
            SetCellValueWithStyle(row, 2, item.Num224, dataStyle);
            SetCellValueWithStyle(row, 3, item.FormName, dataStyle);
            SetCellValueWithStyle(row, 4, item.AuditorName, dataStyle);
            SetCellValueWithStyle(row, 5, item.CollageName, dataStyle);
            SetCellValueWithStyle(row, 6, item.FundName, dataStyle);
            SetCellValueWithStyle(row, 7, item.Details, dataStyle);

            // Set the total debit value
            var totalDebitCell = row.CreateCell(8);
            totalDebitCell.SetCellValue((double)(item.TotalDebit ?? 0));
            totalDebitCell.CellStyle = numericStyle;
            var totalCreditCell = row.CreateCell(9);
            totalCreditCell.SetCellValue((double)(item.TotalCredit ?? 0));
            totalCreditCell.CellStyle = numericStyle;

            // Set the subaccount values (debits)
            for (int i = 0; i < subAccounts.Count; i++)
            {
                var cell = row.CreateCell(subAccountsStartIndex + i);
                var subAccount = item.SubsidaryAccountDtos.ElementAtOrDefault(i);
                cell.SetCellValue((double)(subAccount?.Debit ?? 0));
                cell.CellStyle = numericStyle;
            }

            //set formula cell that sum all subaccount debits
            var sumSubDebitFormula = $"SUM({startSubAccountLetter}{rowIndex + 3}:{endSubAccountLetter}{rowIndex + 3})";
            var sumSubDebitCell = row.CreateCell(subAccountsStartIndex + subAccounts.Count);
            sumSubDebitCell.SetCellFormula(sumSubDebitFormula);
            sumSubDebitCell.CellStyle = numericStyle;

            //setBalance
            var BalanceFormula = $"({sumSubDebitFormula}=Sum($i{rowIndex + 3}:$j{rowIndex + 3}))";
            var BalanceFormulaCell = row.CreateCell(subAccountsStartIndex + subAccounts.Count + 1);
            BalanceFormulaCell.SetCellFormula(BalanceFormula);
            BalanceFormulaCell.CellStyle = numericStyle;

        }

        // Auto-size the columns for better readability
        for (int i = 0; i < totalColumnCount + 1; i++)
        {
            sheet.AutoSizeColumn(i);
        }

        // Convert the workbook to a byte array
        using (var ms = new MemoryStream())
        {
            workbook.Write(ms);
            return ms.ToArray();
        }
    }

    // Helper method to set cell value with style
    private void SetCellValueWithStyle(IRow row, int cellIndex, string value, ICellStyle style)
    {
        var cell = row.CreateCell(cellIndex);
        cell.SetCellValue(value);
        cell.CellStyle = style;
    }

    private List<SubsidaryDailyCollageReportDto> BuildSubsidiaryCollageReport(List<FormDetails> subs, IEnumerable<Collage> collages, IEnumerable<Fund> funds)
    {
        return subs
            .GroupBy(x => x.Form?.CollageId)
            .Select(collageGroup =>
            {
                var collageId = collageGroup.Key;
                var collage = collageId.HasValue ? collages.SingleOrDefault(c => c.Id == collageId.Value) : null;
                return new SubsidaryDailyCollageReportDto
                {
                    CollageId = collageId ?? 0,
                    CollageName = collage?.CollageName ?? string.Empty,
                    Funds = collageGroup.GroupBy(y => y.Form?.FundId)
                        .Where(fundGroup => fundGroup.Key.HasValue)
                        .Select(fundGroup =>
                        {
                            var fundId = fundGroup.Key.Value;
                            var fund = funds.SingleOrDefault(f => f.Id == fundId);
                            return new SubsidaryDailyFundsReportDto()
                            {
                                AccountId = fundGroup.FirstOrDefault()?.AccountId ?? 0,
                                AccountName = fundGroup.FirstOrDefault()?.Account?.AccountName ?? string.Empty,
                                FundId = fundId,
                                FundName = fund?.FundName ?? string.Empty,
                                SubsidaryDetails = fundGroup
                                    .SelectMany(fd => fd.SubsidiaryJournals ?? new List<SubsidiaryJournal>())
                                    .GroupBy(j => j.SubAccountId)
                                    .Select(g =>
                                    {
                                        var subAccount = g.FirstOrDefault()?.SubAccount;
                                        return new SubsidaryDailyDetailsReportDto
                                        {
                                            SubsidaryId = g.Key,
                                            SubsidaryName = subAccount?.SubAccountName ?? string.Empty,
                                            ///  SubsidaryNumber = subAccount?.SubAccountNumber ?? string.Empty,
                                            Credit = g.Sum(j => j.Credit ?? 0),
                                            Debit = g.Sum(j => j.Debit ?? 0)
                                        };
                                    }).OrderBy(x => x.SubsidaryNumber).ToList()
                            };
                        })
                        .Where(x => x != null)
                        .ToList()
                };
            }).ToList();
    }

    private List<SubsidaryDailyDetailsReportDto> CalculateTotalSubsidaries(List<SubsidaryDailyCollageReportDto> subsResult)
    {
        return subsResult
            .SelectMany(x => x.Funds)
            .SelectMany(x => x.SubsidaryDetails)
            .GroupBy(x => x.SubsidaryId)
            .Select(x => new SubsidaryDailyDetailsReportDto()
            {
                SubsidaryId = x.Key,
                SubsidaryName = x.FirstOrDefault()?.SubsidaryName ?? string.Empty,
                SubsidaryNumber = x.FirstOrDefault()?.SubsidaryNumber ?? string.Empty,
                Credit = x.Sum(j => j.Credit ?? 0),
                Debit = x.Sum(j => j.Debit ?? 0)
            }).OrderBy(x => x.SubsidaryNumber).ToList();
    }

    private SubsidaryDailyReportDto BuildEmptyReportDto(GetSubsidartDailyRequest request, IEnumerable<Collage> collages, IEnumerable<Fund> funds, IEnumerable<Account> accounts)
    {
        return new SubsidaryDailyReportDto()
        {
            CollageName = request.CollageId.HasValue ? collages.SingleOrDefault(x => x.Id == request.CollageId.Value)?.CollageName : "الكل",
            FundName = request.FundId.HasValue ? (funds.SingleOrDefault(x => x.Id == request.FundId.Value)?.FundName ?? string.Empty) : "الكل",
            Daily = string.Empty,
            AccountType = !string.IsNullOrEmpty(request.AccountType) ? request.AccountType : "الكل",
            AccountName = request.AccountId.HasValue ? (accounts.SingleOrDefault(x => x.Id == request.AccountId.Value)?.AccountName ?? "الكل") : "الكل",
            Collages = new List<SubsidaryDailyCollageReportDto>(),
            TotalSubsidaries = new List<SubsidaryDailyDetailsReportDto>()
        };
    }

    private SubsidaryDailyReportDto BuildFinalReportDto(GetSubsidartDailyRequest request, IEnumerable<Collage> collages, IEnumerable<Fund> funds, IEnumerable<Account> accounts, List<FormDetails> subs, List<SubsidaryDailyCollageReportDto> subsResult, List<SubsidaryDailyDetailsReportDto> totalSubsidaries)
    {
        return new SubsidaryDailyReportDto()
        {
            CollageName = request.CollageId.HasValue ? collages.Single(x => x.Id == request.CollageId.Value)?.CollageName : "الكل",
            FundName = request.FundId.HasValue ? (funds.SingleOrDefault(x => x.Id == request.FundId.Value)?.FundName ?? string.Empty) : "الكل",
            Daily = subs.FirstOrDefault()?.Form?.Daily?.Name,
            AccountType = !string.IsNullOrEmpty(request.AccountType) ? request.AccountType : "الكل",
            AccountName = request.AccountId.HasValue ? accounts.SingleOrDefault(x => x.Id == request.AccountId.Value)?.AccountName ?? "الكل" : "الكل",
            Collages = subsResult,
            TotalSubsidaries = totalSubsidaries
        };
    }
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
}