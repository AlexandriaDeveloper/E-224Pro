using System.Linq.Expressions;
using System.Reflection;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderByField<T>(this IQueryable<T> source, string fieldName, bool descending = false)
    {
        // جيب الـ Property باستخدام Reflection
        var propertyInfo = typeof(T).GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (propertyInfo == null)
            throw new ArgumentException($"Property '{fieldName}' not found on type '{typeof(T).Name}'");

        // إنشاء Parameter للـ Expression
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyInfo);
        var lambda = Expression.Lambda(property, parameter);

        // اختيار OrderBy أو OrderByDescending
        string methodName = descending ? "OrderByDescending" : "OrderBy";
        var method = typeof(Queryable).GetMethods()
            .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
            .Single()
            .MakeGenericMethod(typeof(T), propertyInfo.PropertyType);

        // استدعاء الـ Method
        return (IQueryable<T>)method.Invoke(null, new object[] { source, lambda });
    }
}