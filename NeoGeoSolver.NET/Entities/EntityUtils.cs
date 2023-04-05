using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public static class EntityUtils
{
  public static ExpressionVector PointOnInPlane(this IEntity entity, Expression t)
  {
#if false
    if (plane == entity.plane)
    {
      return entity.PointOn(t);
    }

    return plane.ToFrom(entity.PointOn(t), entity.plane);
#endif
    return new ExpressionVector(Expression.Zero, Expression.Zero, Expression.Zero);
  }
}
