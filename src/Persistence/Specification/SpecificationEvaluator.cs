using System;
using Core.Models;
using Core.Interfaces.Specification;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Specification;

public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
    {
        var query = inputQuery;

        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }
        if (spec.Criterias.Count > 0)
        {
            foreach (var criteria in spec.Criterias)
            {
                query = query.Where(criteria);
            }
        }
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        if (spec.PaginationEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        // Apply includes
        foreach (var include in spec.Includes)
        {
            query = query.Include(include);
        }

        // Apply then includes
        if (spec.ThenIncludes.Count > 0)
        {
            var lastInclude = spec.Includes.LastOrDefault();
            if (lastInclude != null)
            {
                foreach (var thenInclude in spec.ThenIncludes)
                {
                    query = query.Include(lastInclude).ThenInclude(thenInclude);
                }
            }
        }

        return query;
    }
}
