using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public static class ConstraintExp
{
  public static Expression Angle2d(ExpressionVector d0, ExpressionVector d1, bool angle360 = false)
  {
    var nu = d1.x * d0.x + d1.y * d0.y;
    var nv = d0.x * d1.y - d0.y * d1.x;
    if (angle360)
    {
      return Math.PI - Expression.Atan2(nv, -nu);
    }

    return Expression.Atan2(nv, nu);
  }

  public static Expression PointLineDistance(ExpressionVector p, ExpressionVector p0, ExpressionVector p1)
  {
    return ((p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y + p0.x * p1.y - p1.x * p0.y) / Expression.Sqrt(Expression.Sqr(p1.x - p0.x) + Expression.Sqr(p1.y - p0.y));
  }
}
