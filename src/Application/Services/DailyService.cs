
using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
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
    private readonly IUow _uow;
    public DailyService(IDailyRepository dailyRepository, IUow uow)
    {
        _dailyRepository = dailyRepository;
        _uow = uow;
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
            // var accItem = AccountItemEnum.StateBudget.ToString();
            // var dailyType = DailyTypes.General.ToString();
            // // if i is even 
            // if (i % 2 == 0)
            // {
            //     accItem = AccountItemEnum.SpecialFunds.ToString();
            //     dailyType = DailyTypes.Payroll.ToString();

            // }
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
}
