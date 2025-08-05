using Core.Models;
using Shared.DTOs.FormDtos;

namespace Persistence.Specification;

public class GetDailyFormToExportAsExcelSpecification : Specification<Form>
{
    public GetDailyFormToExportAsExcelSpecification(DailyFormsToExcelRequest request)
    {
        AddInclude(x => x.Collage);
        AddInclude(x => x.Fund);
        AddInclude(x => x.FormDetails);
        AddInclude(x => x.Daily);


        if (request.DailyId.HasValue)
        {
            AddCriteries(x => x.DailyId == request.DailyId!.Value);
        }
        if (request.StartDate.HasValue)
        {
            AddCriteries(x => x.Daily.DailyDate >= request.StartDate!.Value);

        }
        if (request.EndDate.HasValue)
        {
            AddCriteries(x => x.Daily.DailyDate <= request.EndDate!.Value);
        }
        if (!string.IsNullOrEmpty(request.DailyType))
        {
            AddCriteries(x => x.Daily.DailyType == request.DailyType);
        }
        if (request.CollageId.HasValue)
        {
            AddCriteries(x => x.CollageId == request.CollageId.Value);
        }
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.FundId == request.FundId.Value);
        }
        if (request.EntryType.HasValue)
        {
            AddCriteries(x => x.EntryType == (Core.Constants.EntryTypeEnum)request.EntryType.Value);
        }

    }
}
