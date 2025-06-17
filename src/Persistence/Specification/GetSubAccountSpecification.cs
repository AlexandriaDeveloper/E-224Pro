using Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace Persistence.Specification;

public class GetSubAccountSpecification : Specification<SubAccount>
{

    public GetSubAccountSpecification(GetSubAccountRequest request)
    {

        AddInclude(x => x.Account!);

        // if (request.AccountId.HasValue) AddCriteries(x => x.Id == request.Id.Value);
        // if (request.AccountId.HasValue) AddCriteries(x => x.AccountId == request.AccountId.Value);
        if (!request.ParentAccountNumber.IsNullOrEmpty())
        {
            AddCriteries(x => x.Account.AccountNumber == request.ParentAccountNumber);
        }
        if (!request.ParentAccountName.IsNullOrEmpty())
        {
            AddCriteries(x => x.Account.AccountName!.Contains(request.ParentAccountName));
        }
        if (!request.SubAccountName.IsNullOrEmpty())
        {
            AddCriteries(x => x.SubAccountName!.Contains(request.SubAccountName!));
        }
        if (!request.SubAccountNumber.IsNullOrEmpty())
        {
            AddCriteries(x => x.SubAccountNumber!.Contains(request.SubAccountNumber!));
        }
        if (request.PageIndex.HasValue && request.PageSize.HasValue)
        {
            ApplyPaging(request.PageIndex.Value, request.PageSize.Value);
        }
    }

}
public class GetSubAccountCountAsyncSpecification : Specification<SubAccount>
{

    public GetSubAccountCountAsyncSpecification(GetSubAccountRequest request)
    {
        AddInclude(x => x.Account!);
        if (!request.ParentAccountNumber.IsNullOrEmpty())
        {
            AddCriteries(x => x.Account.AccountNumber == request.ParentAccountNumber);
        }
        if (!request.ParentAccountName.IsNullOrEmpty())
        {
            AddCriteries(x => x.Account.AccountName!.Contains(request.ParentAccountName));
        }
        if (!request.SubAccountName.IsNullOrEmpty())
        {
            AddCriteries(x => x.SubAccountName!.Contains(request.SubAccountName!));
        }
        if (!request.SubAccountNumber.IsNullOrEmpty())
        {
            AddCriteries(x => x.SubAccountNumber!.Contains(request.SubAccountNumber!));
        }

    }

}