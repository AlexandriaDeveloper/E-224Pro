using Core.Models;
using Microsoft.IdentityModel.Tokens;
using Shared.Contracts.FormDetailsRequest;
using Shared.Contracts.FormRequests;

namespace Persistence.Specification;

public class GetFormDetailsBySpecification : Specification<FormDetails>
{
    public GetFormDetailsBySpecification(GetFormDetailsRequest request)
    {
        AddInclude(x => x.Account!);
        AddInclude(x => x.Form!);
        AddInclude(x => x.Form!.Collage!);
        AddInclude(x => x.Form!.Fund!);
        AddInclude(x => x.Form!.Daily!);
        if (request.FormId.HasValue)
        {
            AddCriteries(x => x.FormId == request.FormId.Value);

        }
        if (request.AccountId.HasValue)
        {
            //  AddInclude(x => x.Account);
            AddCriteries(x => x.Account!.Id == request.AccountId.Value);
        }
        if (!string.IsNullOrEmpty(request.AccountName))
        {
            //   AddInclude(x => x.Account);
            AddCriteries(x => x.Account!.Name == request.AccountName);
        }
        if (!string.IsNullOrEmpty(request.FormName))
        {
            AddCriteries(x => x.Form!.FormName == request.FormName);
        }
        if (request.AccountId.HasValue)
        {
            //   AddInclude(x => x.Account);
            AddCriteries(x => x.Account!.Id == request.AccountId);
        }
        if (!string.IsNullOrEmpty(request.CollageName))
        {
            // AddInclude(x => x.Forms.Collage);
            AddCriteries(x => x.Form!.Collage!.CollageName == request.CollageName);
        }
        if (!string.IsNullOrEmpty(request.FundName))
        {
            //AddInclude(x => x.Forms.Fund);
            AddCriteries(x => x.Form!.Fund!.FundName == request.FundName);
        }
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.Form!.FundId == request.FundId.Value);
        }
        if (!string.IsNullOrEmpty(request.FormNum55))
        {
            AddCriteries(x => x.Form!.Num55 == request.FormNum55);
        }
        if (!string.IsNullOrEmpty(request.FormNum224))
        {
            AddCriteries(x => x.Form!.Num224 == request.FormNum224);
        }
        if (request.StartFrom.HasValue && !request.EndTo.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate >= request.StartFrom.Value);
        }

        if (!request.StartFrom.HasValue && request.EndTo.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate <= request.EndTo.Value);
        }

        if (request.StartFrom.HasValue && request.EndTo.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate >= request.StartFrom.Value && x.Form.Daily.DailyDate <= request.EndTo);
        }
        if (request.DailyId.HasValue)
        {
            AddCriteries(x => x.Form!.DailyId == request.DailyId.Value);
        }

        if (request.PageIndex.HasValue && request.PageSize.HasValue)
        {
            ApplyPaging(request.PageIndex.Value, request.PageSize.Value);
        }



    }
}