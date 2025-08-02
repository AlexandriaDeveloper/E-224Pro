using System;
using Azure.Core;
using Core.Constants;
using Core.Models;
using Shared.Contracts;
using Shared.DTOs.FormDtos;

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
        // if (!string.IsNullOrEmpty(request.AccountItem))
        // {
        //     AddCriteries(x => x.AccountItem == request.AccountItem);

        // }
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
        // if (!string.IsNullOrEmpty(request.AccountItem))
        // {
        //     AddCriteries(x => x.AccountItem == request.AccountItem);

        // }

    }




}
public class SubsidaryToExcelSpecification : Specification<FormDetails>
{
    public SubsidaryToExcelSpecification(SubsidaryToExcelRequest request)
    {
        AddInclude(x => x.Form.Daily);
        AddInclude(x => x.Form);
        AddInclude(x => x.Form.Collage);
        AddInclude(x => x.Form.Fund);
        AddInclude(x => x.SubsidiaryJournals);
        AddInclude(x => x.Account);

        if (request.Id.HasValue)
        {
            AddCriteries(x => x.Id == request.Id.Value);
        }
        if (request.DailyId.HasValue)
        {
            AddCriteries(x => x.Form.Daily.Id == request.DailyId.Value);
        }
        if (request.CollageId.HasValue)
        {
            AddCriteries(x => x.Form.Collage.Id == request.CollageId.Value);
        }
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.Form.Fund.Id == request.FundId.Value);
        }
        if (request.AccountId.HasValue)
        {
            AddCriteries(x => x.AccountId == request.AccountId.Value);
        }
        if (request.EntryType.HasValue)
        {
            AddCriteries(x => x.Form.EntryType == (EntryTypeEnum)request.EntryType.Value);
        }




    }
}