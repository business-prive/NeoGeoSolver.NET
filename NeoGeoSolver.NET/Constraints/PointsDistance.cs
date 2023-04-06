using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsDistance : Value
{
  public ExpressionVector p0Exp
  {
    get
    {
      return Point0.Expr;
    }
  }

  public ExpressionVector p1Exp
  {
    get
    {
      return Point1.Expr;
    }
  }

  public Point Point0 { get; }
  public Point Point1 { get; }

  public PointsDistance(Point pt0, Point pt1)
  {
    Point0 = pt0;
    Point1 = pt1;
    Satisfy();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return (p1Exp - p0Exp).Magnitude() - value.Expr;
    }
  }
}
