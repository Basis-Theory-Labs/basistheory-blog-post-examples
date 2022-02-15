using System.Linq.Expressions;
using LanguageExt;

namespace BasisTheory.LuceneSearchingExample.Extensions.Expressions;

public static class ExpressionUtility
{
    private static Expression<T> Compose<T>(
        this Expression<T> first,
        Expression<T> second,
        Func<Expression, Expression, Expression> merge)
    {
        // build parameter map (from parameters of second to parameters of first)
        var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] })
            .ToDictionary(p => p.s, p => p.f);

        // replace parameters in the second lambda expression with parameters from the first
        var secondBody = ExpressionParameterRebinder.ReplaceParameters(map, second.Body);

        // apply composition of lambda expression bodies to parameters from the first expression
        return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
    }

    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return first.IsNull() ? second :
            second.IsNull() ? first :
            first.Compose(second, Expression.And);
    }

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return first.IsNull() ? second :
            second.IsNull() ? first :
            first.Compose(second, Expression.Or);
    }
}