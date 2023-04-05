using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointCircleDistance : Value
{
  private readonly Point _pt;
  private readonly Circle _circle;

  public PointCircleDistance(Point pt, Circle c)
  {
    _pt = pt;
    _circle = c;
    Satisfy();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var pPos = _pt.exp;
      var cCen = _circle.CentreExpr();
      var cRad = _circle.RadiusExpr();

      yield return (pPos - cCen).Magnitude() - cRad - value.Expr;
    }
  }
}
