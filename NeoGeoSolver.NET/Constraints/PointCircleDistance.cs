using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointCircleDistance : Value
{
  public readonly Point Point;
  public readonly Circle Circle;

  public PointCircleDistance(Point pt, Circle circle)
  {
    Point = pt;
    Circle = circle;
    Satisfy();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var pPos = Point.exp;
      var cCen = Circle.CentreExpr();
      var cRad = Circle.RadiusExpr();

      yield return (pPos - cCen).Magnitude() - cRad - value.Expr;
    }
  }
}
