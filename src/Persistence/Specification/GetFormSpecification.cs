using Core.Models;
using Microsoft.IdentityModel.Tokens;
using Shared.Contracts.FormRequests;

namespace Persistence.Specification;

public class GetFormSpecification : Specification<Form>
{
    public GetFormSpecification(GetFormRequest request)
    {
        // AddInclude(x => x.FormDetails);
        AddInclude(x => x.Collage);
        AddInclude(x => x.Fund);

        if (request.Id.HasValue)
        {
            AddCriteries(x => x.Id == request.Id!.Value);

        }


        if (request.CollageId.HasValue)
        {
            AddCriteries(x => x.CollageId == request.CollageId.Value);
        }
        if (!request.FormName.IsNullOrEmpty())
        {
            AddCriteries(x => x.FormName.Contains(request.FormName!));
        }
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.FundId == request.FundId.Value);
        }
        if (!request.Num224.IsNullOrEmpty())
        {
            AddCriteries(x => x.Num224 == request.Num224);
        }
        if (!request.Num55.IsNullOrEmpty())
        {
            AddCriteries(x => x.Num55 == request.Num55);
        }
        if (request.DailyId.HasValue)
        {
            AddCriteries(x => x.DailyId == request.DailyId.Value);
        }
        if (!request.AuditorName.IsNullOrEmpty())
        {
            AddCriteries(x => x.AuditorName!.Contains(request.AuditorName!));
        }
        if (!request.Details.IsNullOrEmpty())
        {
            AddCriteries(x => x.Details!.Contains(request.Details!));
        }


        AddOrderByDescending(x => x.Id);

        if (request.PageIndex.HasValue)
        {
            ApplyPaging(request.PageIndex.Value, request.PageSize!.Value);
        }

    }



}
public class GetFormCountAsyncSpecification : Specification<Form>
{
    public GetFormCountAsyncSpecification(GetFormRequest request)
    {
        if (request.Id.HasValue)
        {
            AddCriteries(x => x.Id == request.Id!.Value);

        }


        if (request.CollageId.HasValue)
        {
            AddCriteries(x => x.CollageId == request.CollageId.Value);
        }
        if (!request.FormName.IsNullOrEmpty())
        {
            AddCriteries(x => x.FormName.Contains(request.FormName!));
        }
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.FundId == request.FundId.Value);
        }
        if (!request.Num224.IsNullOrEmpty())
        {
            AddCriteries(x => x.Num224 == request.Num224);
        }
        if (!request.Num55.IsNullOrEmpty())
        {
            AddCriteries(x => x.Num55 == request.Num55);
        }
        if (request.DailyId.HasValue)
        {
            AddCriteries(x => x.DailyId == request.DailyId.Value);
        }
        if (!request.AuditorName.IsNullOrEmpty())
        {
            AddCriteries(x => x.AuditorName!.Contains(request.AuditorName!));
        }
        if (!request.Details.IsNullOrEmpty())
        {
            AddCriteries(x => x.Details!.Contains(request.Details!));
        }


    }


}
