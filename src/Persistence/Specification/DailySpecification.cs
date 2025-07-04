using System;
using Azure.Core;
using Core.Models;
using Shared.Contracts;

namespace Persistence.Specification;

public class DailySpecification : Specification<Daily>
{
    public DailySpecification(GetDailyRequest request)
    {


        if (!string.IsNullOrEmpty(request.Name))
        {
            AddCriteries(x => x.Name.Contains(request.Name));
        }

        if (request.StartDate.HasValue)
        {
            AddCriteries(x => x.DailyDate >= request.StartDate);

        }
        if (request.EndDate.HasValue)
        {
            AddCriteries(x => x.DailyDate <= request.EndDate);
        }


        if (!string.IsNullOrEmpty(request.DailyType))
        {
            AddCriteries(x => x.DailyType == request.DailyType);

        }
        if (!string.IsNullOrEmpty(request.AccountItem))
        {
            AddCriteries(x => x.AccountItem == request.AccountItem);

        }
        AddOrderByDescending(x => x.Id);
        if (request.PageIndex.HasValue && request.PageSize.HasValue)
            ApplyPaging(request.PageIndex.Value, request.PageSize.Value);


    }



}
public class DailyCountAsyncSpecification : Specification<Daily>
{
    public DailyCountAsyncSpecification(GetDailyRequest request)
    {


        if (!string.IsNullOrEmpty(request.Name))
        {
            AddCriteries(x => x.Name.Contains(request.Name));
        }

        if (request.StartDate.HasValue)
        {
            AddCriteries(x => x.DailyDate >= request.StartDate);

        }
        if (request.EndDate.HasValue)
        {
            AddCriteries(x => x.DailyDate <= request.EndDate);
        }



        if (!string.IsNullOrEmpty(request.DailyType))
        {
            AddCriteries(x => x.DailyType == request.DailyType);

        }
        if (!string.IsNullOrEmpty(request.AccountItem))
        {
            AddCriteries(x => x.AccountItem == request.AccountItem);

        }

    }




}
