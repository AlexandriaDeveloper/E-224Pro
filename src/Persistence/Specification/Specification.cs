using System;
using System.Linq.Expressions;
using Core.Interfaces.Specification;

namespace Persistence.Specification;

public class Specification<TEntity> : ISpecification<TEntity>
{
    // public int PageSize { get; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool PaginationEnabled { get; set; } = false;

    public List<string> IncludeStrings { get; }

    public Expression<Func<TEntity, bool>> Criteria { get; }

    public List<Expression<Func<TEntity, bool>>> Criterias { get; } = new List<Expression<Func<TEntity, bool>>>();

    public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();
    public List<Expression<Func<object, object>>> ThenIncludes { get; } = new List<Expression<Func<object, object>>>();

    public Expression<Func<TEntity, object>> OrderBy { get; private set; }

    public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

    public Specification()
    {

    }
    public Specification(Expression<Func<TEntity, bool>> criteria)
    {
        if (criteria != null)
        {
            Criteria = criteria;
        }
    }

    protected void AddCriteries(Expression<Func<TEntity, bool>> criteria)
    {
        this.Criterias.Add(criteria);
    }
    protected void AddInclude(Expression<Func<TEntity, object>> include)
    {
        this.Includes.Add(include);
    }

    protected void AddThenInclude(Expression<Func<object, object>> thenInclude)
    {
        this.ThenIncludes.Add(thenInclude);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }
    //I Need Chaining method for ThenInclude 


    protected void ApplyPaging(int pageIndex, int pageSize)
    {
        if (pageIndex < 0)
        {
            pageIndex = 0;
        }
        Skip = pageSize * (pageIndex);
        Take = pageSize;
        PaginationEnabled = true;


    }
}
