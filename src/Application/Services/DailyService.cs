
using System.Data;
using Application.Helper;
using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Specification;
using Shared.Common;
using Shared.Constants.Enums;
using Shared.Contracts;
using Shared.DTOs;



namespace Application.Services;

public class DailyService
{
    private readonly IDailyRepository _dailyRepository;
    private readonly IFormRepository _formRepository;
    private readonly IFormDetailsRepository _formDetailsRepository;
    private readonly ICollageRepository _collageRepository;
    private readonly IFundRepository _fundRepository;
    private readonly IUow _uow;
    public DailyService(IDailyRepository dailyRepository, IFormRepository formRepository, IFormDetailsRepository formDetailsRepository, ICollageRepository collageRepository, IFundRepository fundRepository, IUow uow)
    {
        _dailyRepository = dailyRepository;
        this._formRepository = formRepository;
        this._formDetailsRepository = formDetailsRepository;
        this._collageRepository = collageRepository;
        this._fundRepository = fundRepository;
        _uow = uow;
    }
    private async Task<List<Collage>> GetCollages()
    {
        var collages = await _collageRepository.GetQueryable().ToListAsync();
        if (collages == null || collages.Count == 0)
        {
            throw new Exception("Could not find any collages");
        }
        return collages;
    }
    private async Task<List<Fund>> GetFunds()
    {
        var funds = await _fundRepository.GetQueryable().ToListAsync();
        if (funds == null || funds.Count == 0)
        {
            throw new Exception("Could not find any funds");
        }
        return funds;
    }

    //Post Daily
    public async Task CreateDailyAsync(PostDailyRequest daily, CancellationToken cancellationToken = default)
    {
        if (daily == null)
        {
            throw new ArgumentNullException(nameof(daily));
        }
        var dailyToDb = new Daily()
        {
            Name = daily.Name,
            DailyDate = daily.DailyDate,
            AccountItem = daily.AccountItem.ToString()!,

            DailyType = daily.DailyType.ToString()!

        };
        await _dailyRepository.AddAsync(dailyToDb);
        await _uow.CommitAsync(cancellationToken);
    }
    public async Task UpdateDailyAsync(int id, PutDailyRequest request, CancellationToken cancellationToken = default)
    {
        var daily = await _dailyRepository.GetById(id, true, cancellationToken);
        if (daily == null)
        {
            throw new ArgumentNullException(nameof(daily));
        }



        if (!request.Name.IsNullOrEmpty())
            daily.Name = request.Name;

        if (request.DailyDate.HasValue)
        {
            daily.DailyDate = request.DailyDate.Value;
        }

        if (!request.DailyType.IsNullOrEmpty())
            daily.DailyType = request.DailyType;

        if (!request.AccountItem.IsNullOrEmpty())
            daily.AccountItem = request.AccountItem;




        await _dailyRepository.UpdateAsync(daily);
        await _uow.CommitAsync(cancellationToken);
    }
    public async Task<DailyDto> GetDailyById(int id, CancellationToken cancellationToken = default)
    {

        var daily = await _dailyRepository.GetById(id, true, cancellationToken);
        if (daily == null)
        {
            throw new Exception("Could not find");
        }
        // Return the daily
        var dailyToReturn = new DailyDto(daily);
        return dailyToReturn;

    }

    public async Task<PaginatedResult<DailyDto>> GetDailiesBySpec(GetDailyRequest request, CancellationToken cancellationToken = default)
    {
        var spec = new DailySpecification(request);

        var dailies = _dailyRepository.GetQueryable(spec).Include(x => x.Forms).ThenInclude(x => x.FormDetails);
        var dailyCountSpec = new DailyCountAsyncSpecification(request);
        var dailyCountResult = await _dailyRepository.CountAsync(dailyCountSpec);

        // Return the daily

        var dailiesResponse = dailies.Select(x => new DailyDto(x!)).ToList();


        return PaginatedResult<DailyDto>.Create(dailiesResponse, request.PageIndex, request.PageSize, dailyCountResult);

    }

    public async Task DeleteDailyAsync(int id, CancellationToken cancellationToken = default)
    {
        var daily = await _dailyRepository.GetById(id);
        if (daily == null)
        {
            throw new ArgumentNullException(nameof(daily));
        }
        _dailyRepository.Delete(daily);
        await _uow.CommitAsync(cancellationToken);
    }
    public async Task TestCreate30000DailyAsync(CancellationToken cancellationToken = default)
    {
        List<Daily> dailies = new List<Daily>();
        for (int i = 0; i < 30000; i++)
        {
            int random2 = new Random().Next(0, 2);

            var accItem = Enum.GetNames(typeof(AccountItemEnum)).GetValue(random2)!.ToString();
            var dailyType = Enum.GetNames(typeof(DailyTypes)).GetValue(random2)!.ToString();



            dailies.Add(new Daily()
            {
                AccountItem = accItem,
                Name = $"Test Daily {i}",
                DailyDate = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                DailyType = dailyType!
            });
        }
        await _dailyRepository.AddRange2Async(dailies);
        await _uow.CommitAsync(cancellationToken);

    }

    public async Task UploadExcelFormAsync(int dailyId, IFormFile file, HttpContext context, CancellationToken cancellationToken = default)
    {
        var collages = await GetCollages();
        if (collages == null || collages.Count == 0)
        {
            throw new Exception("Could not find any collages");
        }
        var funds = await GetFunds();
        if (funds == null || funds.Count == 0)
        {
            throw new Exception("Could not find any funds");
        }
        var daily = await _dailyRepository.GetById(dailyId, true);
        if (daily == null)
        {
            throw new ArgumentNullException(nameof(daily));
        }
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is null or empty", nameof(file));
        }

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var tempPath = Path.GetTempPath();
        if (tempPath == null)
        {
            throw new InvalidOperationException("Could not get the content root path.");
        }
        var filePath = Path.Combine(tempPath, "uploads", fileName);
        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? throw new InvalidOperationException("Could not create directory for file upload."));
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        // Process the uploaded Excel file
        var excelService = new NpoiService(filePath);
        var sheetHeader1 = excelService.GetSheetHeader(0, "Sheet1");
        List<short> debitList = new List<short>();
        List<short> creditList = new List<short>();
        int maxFormId = await _formRepository.CountAsync(null) > 0 ? _formRepository.GetQueryable().Max(x => x.Id) + 1 : 1;
        int maxFormDetailsId = await _formDetailsRepository.CountAsync(null) > 0 ? _formDetailsRepository.GetQueryable().Max(x => x.Id) + 1 : 1;
        //if header start with 10 list its index in debit list if start with 20 list its index in credit
        for (int i = 0; i < sheetHeader1.Length; i++)
        {
            if (sheetHeader1[i].StartsWith("10"))
            {
                debitList.Add((short)i);
            }
            else if (sheetHeader1[i].StartsWith("20"))
            {
                creditList.Add((short)i);
            }
        }
        var sheetHeader2 = excelService.GetSheetHeader(1, "Sheet1");
        var sheetData = excelService.GetSheetData(2, "Sheet1");

        var forms = new List<Form>();
        foreach (var row in sheetData)
        {
            if (row == null || row.Length == 0 || row[1].ToString() == null || row[3].ToString().Trim() == string.Empty)
            {
                continue; // Skip empty rows
            }
            if (row[0].ToString() == "الاجمالى")
            {
                break; // Skip the total row
            }
            var collageId = collages.FirstOrDefault(c => c.CollageName == row[5].ToString().Trim())?.Id;
            if (collageId == null)
            {
                throw new Exception($"Collage '{row[5].ToString().Trim()}' not found.");
            }
            var fundId = funds.FirstOrDefault(f => f.FundName == row[6].ToString().Trim() && f.CollageId == collageId)?.Id;
            if (fundId == null)
            {
                throw new Exception($"Fund '{row[6].ToString().Trim()}' not found.");
            }


            var form = new Form
            {
                Id = maxFormId,
                DailyId = dailyId,
                Num55 = row[1].ToString().Trim(),
                Num224 = row[2].ToString().Trim(),
                FormName = row[3].ToString().Trim(),
                AuditorName = row[4].ToString().Trim(),
                CollageId = collageId,
                FundId = fundId,
                Details = row[7].ToString().Trim(),

            };
            form.FormDetails = new List<FormDetails>();

            foreach (var i in debitList)
            {
                //check list if the value is null or empty then skip it
                if (string.IsNullOrWhiteSpace(row[i].ToString()) || row[i].ToString() == "0")
                {

                    continue; // Skip empty debit accounts
                }
                form.FormDetails.Add(new FormDetails()
                {
                    Id = maxFormDetailsId++,
                    //try parse to decimal if not then skip it
                    Debit = decimal.TryParse(row[i].ToString().Trim(), out var debitValue) ? debitValue : 0,
                    AccountId = int.Parse(sheetHeader1[i])

                });

            }
            foreach (var i in creditList)
            {
                //check list if the value is null or empty then skip it
                if (string.IsNullOrWhiteSpace(row[i].ToString()) || row[i].ToString() == "0")
                {
                    continue; // Skip empty credit accounts
                }
                form.FormDetails.Add(new FormDetails()
                {
                    Id = maxFormDetailsId++,
                    //try parse to decimal if not then skip it
                    Credit = decimal.TryParse(row[i].ToString().Trim(), out var creditValue) ? creditValue : 0,
                    AccountId = int.Parse(sheetHeader1[i])
                });
            }
            form.TotalDebit = form.FormDetails.Where(x => x.Debit > 0).Sum(x => x.Debit);
            form.TotalCredit = form.FormDetails.Where(x => x.Credit > 0).Sum(x => x.Credit);
            forms.Add(form);

        }


        await _formRepository.AddRange2Async(forms);
        // Commit the changes to the database

        await _uow.CommitAsync(cancellationToken);

        // Optionally, you can return a response indicating success
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsync("Excel file uploaded and processed successfully.");

    }
}
