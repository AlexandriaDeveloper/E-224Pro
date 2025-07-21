using Core.Constants;
using Core.Models;
using Shared.Contracts.FormDetailsRequest;

namespace Persistence.Specification;

public class SearchFormAsyncSpecification : Specification<Form>
{
    public SearchFormAsyncSpecification(SearchFormRequest request)
    {
        AddInclude(x => x.Daily);
        AddInclude(x => x.FormDetails);
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
        if (request.EntryType.HasValue)
        {
            AddCriteries(x => x.EntryType == (EntryTypeEnum)request.EntryType.Value);
        }
        if (!string.IsNullOrEmpty(request.AuditorName))
        {
            AddCriteries(x => x.AuditorName!.Contains(request.AuditorName!));
        }
        if (!string.IsNullOrEmpty(request.Details))
        {
            AddCriteries(x => x.Details!.Contains(request.Details!));
        }
        if (!string.IsNullOrEmpty(request.DailyType))
        {
            AddCriteries(x => x.Daily!.DailyType == request.DailyType);
        }
        if (request.StartFrom.HasValue)
        {
            AddCriteries(x => x.Daily.DailyDate >= request.StartFrom.Value);
        }
        if (request.EndTo.HasValue)
        {
            AddCriteries(x => x.Daily.DailyDate <= request.EndTo.Value);
        }

        if (request.PageIndex.HasValue && request.PageSize.HasValue)
        {
            ApplyPaging(request.PageIndex.Value, request.PageSize.Value);
        }
    }
}

public class SearchFormCountAsyncSpecification : Specification<Form>
{
    public SearchFormCountAsyncSpecification(SearchFormRequest request)
    {

        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

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
        if (request.EntryType.HasValue)
        {
            AddCriteries(x => x.EntryType == (EntryTypeEnum)request.EntryType.Value);
        }
        if (!string.IsNullOrEmpty(request.AuditorName))
        {
            AddCriteries(x => x.AuditorName!.Contains(request.AuditorName!));
        }
        if (!string.IsNullOrEmpty(request.Details))
        {
            AddCriteries(x => x.Details!.Contains(request.Details!));
        }
        if (!string.IsNullOrEmpty(request.DailyType))
        {
            AddCriteries(x => x.Daily!.DailyType == request.DailyType);
        }
        if (request.StartFrom.HasValue)
        {
            AddCriteries(x => x.Daily.DailyDate >= request.StartFrom.Value);
        }
        if (request.EndTo.HasValue)
        {
            AddCriteries(x => x.Daily.DailyDate <= request.EndTo.Value);
        }

    }
}