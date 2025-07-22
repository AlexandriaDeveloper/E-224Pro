using Core.Models;
using Microsoft.IdentityModel.Tokens;
using Shared.Contracts.FormDetailsRequest;
using Shared.Contracts.FormRequests;

namespace Persistence.Specification;

public class GetFormWithDetailsSpecification : Specification<Form>
{
    public GetFormWithDetailsSpecification(GetFormDetailsRequest request)
    {
        AddInclude(x => x.FormDetails);
        AddInclude(x => x.Collage!);
        AddInclude(x => x.Fund!);





        if (request.FormId.HasValue)
        {
            AddCriteries(x => x.Id == request.FormId!.Value);

        }


        if (request.CollageId.HasValue)
        {
            AddCriteries(x => x.CollageId == request.CollageId.Value);
            if (!string.IsNullOrEmpty(request.FormName))
            {
                AddCriteries(x => x.FormName.Contains(request.FormName!));
            }
            if (request.FundId.HasValue)
            {
                AddCriteries(x => x.FundId == request.FundId.Value);
            }
            if (!string.IsNullOrEmpty(request.CollageName))
            {
                AddCriteries(x => x.Collage!.Name.Contains(request.CollageName!));
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



            if (request.PageIndex.HasValue)
            {
                ApplyPaging(request.PageIndex.Value, request.PageSize!.Value);
            }

        }
    }



}