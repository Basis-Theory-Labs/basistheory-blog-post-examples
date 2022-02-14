using System.Linq.Expressions;

namespace BasisTheory.LuceneSearchingExample.Extensions.Expressions;

public class ExpressionParameterRebinder : ExpressionVisitor
{
    private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

    private ExpressionParameterRebinder(Dictionary<ParameterExpression, ParameterExpression>? map)
    {
        _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
    }

    public static Expression ReplaceParameters(
        Dictionary<ParameterExpression, ParameterExpression>? map,
        Expression exp)
    {
        return new ExpressionParameterRebinder(map).Visit(exp);
    }

    protected override Expression VisitParameter(ParameterExpression p)
    {
        if (_map.TryGetValue(p, out var replacement))
        {
            p = replacement;
        }

        return base.VisitParameter(p);
    }
}