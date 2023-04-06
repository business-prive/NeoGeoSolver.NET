using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointCircleDistance : Value
{
  private readonly Point _point;
  private readonly Circle _circle;

  public PointCircleDistance(Point pt, Circle circle)
  {
    _point = pt;
    _circle = circle;
    Satisfy();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var pPos = _point.Expr;
      var cCen = _circle.CentreExpr();
      var cRad = _circle.RadiusExpr();

      yield return (pPos - cCen).Magnitude() - cRad - value.Expr;
    }
  }
}
