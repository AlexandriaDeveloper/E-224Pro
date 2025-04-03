using System;
using System.Linq.Expressions;

namespace Core.Interfaces.Specification;

public interface ISpecification<TEntity>
{
    int Skip { get; }
    int Take { get; }
    bool PaginationEnabled { get; }
    List<string> IncludeStrings { get; }
    Expression<Func<TEntity, bool>> Criteria { get; }
    List<Expression<Func<TEntity, bool>>> Criterias { get; }
    List<Expression<Func<TEntity, object>>> Includes { get; }
    List<Expression<Func<object, object>>> ThenIncludes { get; }

    Expression<Func<TEntity, object>> OrderBy { get; }
    Expression<Func<TEntity, object>> OrderByDescending { get; }
}
