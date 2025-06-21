using Core.Models;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs.FormDtos;

namespace Persistence.Specification;

public class GetSubsidaryDailyBySpecification : Specification<SubsidiaryJournal>
{

    public GetSubsidaryDailyBySpecification(GetSubsidartDailyRequest request)
    {
        AddInclude(x => x.FormDetails);
        AddInclude(x => x.FormDetails.Form!);
        AddInclude(x => x.FormDetails.Form!.Daily!);
        AddInclude(x => x.FormDetails.Form!.Collage!);
        AddInclude(x => x.FormDetails.Form!.Fund!);

        if (request.AccountId.HasValue)
        {
            AddCriteries(x => x.FormDetails.AccountId == request.AccountId);
        }

        if (!request.CollageId.HasValue)
        {
            AddCriteries(x => x.FormDetails.Form.CollageId! == request.CollageId);
        }
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.FormDetails.Form!.FundId! == request.FundId);
        }

        if (request.DailyId.HasValue)
        {

            AddCriteries(x => x.FormDetails.Form!.DailyId == request.DailyId.Value);
        }
        if (!request.DailyType.IsNullOrEmpty())
        {

            AddCriteries(x => x.FormDetails.Form.DailyId == request.DailyId.Value);
        }




    }
}