using Core.Constants;
using Core.Models;
using Shared.DTOs.FormDtos;

namespace Persistence.Specification;

public class GetSubsidaryDailyBySpecification : Specification<FormDetails>
{

    public GetSubsidaryDailyBySpecification(GetSubsidartDailyRequest request)
    {
        AddInclude(x => x.SubsidiaryJournals!);
        AddInclude(x => x.Form!);
        AddInclude(x => x.Form!.Daily!);
        AddInclude(x => x.Form!.Collage!);
        AddInclude(x => x.Form!.Fund!);
        AddInclude(x => x.Account);
        AddInclude(x => x.Account.SubAccounts);

        if (request.AccountId.HasValue)
        {
            AddCriteries(x => x.AccountId == request.AccountId);
        }

        if (request.CollageId.HasValue)
        {
            AddCriteries(x => x.Form.CollageId! == request.CollageId);
        }
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.Form!.FundId! == request.FundId);
        }
        if (!string.IsNullOrEmpty(request.DailyType))
        {

            AddCriteries(x => x.Form.Daily.DailyType == request.DailyType);
        }

        if (request.DailyId.HasValue)
        {

            AddCriteries(x => x.Form!.DailyId == request.DailyId.Value);
        }

        if (request.EntryType.HasValue)
        {
            AddCriteries(x => x.Form!.EntryType == (EntryTypeEnum)request.EntryType.Value);
        }
        if (request.StartDate.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate >= request.StartDate);
        }

        if (request.EndDate.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate <= request.EndDate);
        }

        if (!string.IsNullOrEmpty(request.Num55))
        {
            AddCriteries(x => x.Form!.Num55.Contains(request.Num55));
        }
        if (!string.IsNullOrEmpty(request.Num224))
        {
            AddCriteries(x => x.Form!.Num224.Contains(request.Num224));
        }



    }
}