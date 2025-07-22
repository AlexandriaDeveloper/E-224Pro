using Core.Models;
using Shared.Contracts.FormDetailsRequest;
using Shared.Contracts.FormRequests;

namespace Persistence.Specification;

public class GetFormSpecification : Specification<Form>
{
    public GetFormSpecification(SearchFormRequest request)
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
        if (!string.IsNullOrEmpty(request.FormName))
        {
            AddCriteries(x => x.FormName.Contains(request.FormName!));
        }
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.FundId == request.FundId.Value);
        }
        if (!string.IsNullOrEmpty(request.FormNum224))
        {
            AddCriteries(x => x.Num224 == request.FormNum224);
        }

        if (!string.IsNullOrEmpty(request.FormNum55))
        {
            AddCriteries(x => x.Num55 == request.FormNum55);
        }
        if (request.DailyId.HasValue)
        {
            AddCriteries(x => x.DailyId == request.DailyId.Value);
        }
        if (!string.IsNullOrEmpty(request.AuditorName))
        {
            AddCriteries(x => x.AuditorName!.Contains(request.AuditorName!));
        }
        if (request.EntryType.HasValue)
        {
            AddCriteries(x => x.EntryType == (Core.Constants.EntryTypeEnum)request.EntryType.Value);
        }
        if (!string.IsNullOrEmpty(request.Details))
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
    public GetFormCountAsyncSpecification(SearchFormRequest request)
    {
        if (request.Id.HasValue)
        {
            AddCriteries(x => x.Id == request.Id!.Value);
        }
        if (request.CollageId.HasValue)
        {
            AddCriteries(x => x.CollageId == request.CollageId.Value);
        }
        if (!string.IsNullOrEmpty(request.FormName))
        {
            AddCriteries(x => x.FormName.Contains(request.FormName!));
        }
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.FundId == request.FundId.Value);
        }
        if (!string.IsNullOrEmpty(request.FormNum224))
        {
            AddCriteries(x => x.Num224 == request.FormNum224);
        }
        if (!string.IsNullOrEmpty(request.FormNum55))
        {
            AddCriteries(x => x.Num55 == request.FormNum55);
        }
        if (request.DailyId.HasValue)
        {
            AddCriteries(x => x.DailyId == request.DailyId.Value);
        }
        if (!string.IsNullOrEmpty(request.AuditorName))
        {
            AddCriteries(x => x.AuditorName!.Contains(request.AuditorName!));
        }
        if (!string.IsNullOrEmpty(request.Details))
        {
            AddCriteries(x => x.Details!.Contains(request.Details!));
        }
    }
}
public class GetFormCountSpecification : Specification<Form>
{
    public GetFormCountSpecification(GetFormRequest request)
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
        if (!string.IsNullOrEmpty(request.FormName))
        {
            AddCriteries(x => x.FormName.Contains(request.FormName!));
        }
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.FundId == request.FundId.Value);
        }
        if (!string.IsNullOrEmpty(request.FormNum224))
        {
            AddCriteries(x => x.Num224 == request.FormNum224);
        }
        if (!string.IsNullOrEmpty(request.FormNum55))
        {
            AddCriteries(x => x.Num55 == request.FormNum55);
        }
        if (request.DailyId.HasValue)
        {
            AddCriteries(x => x.DailyId == request.DailyId.Value);
        }
        if (!string.IsNullOrEmpty(request.AuditorName))
        {
            AddCriteries(x => x.AuditorName!.Contains(request.AuditorName!));
        }
        if (request.EntryType.HasValue)
        {
            AddCriteries(x => x.EntryType == (Core.Constants.EntryTypeEnum)request.EntryType.Value);
        }
        if (!string.IsNullOrEmpty(request.Details))
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
