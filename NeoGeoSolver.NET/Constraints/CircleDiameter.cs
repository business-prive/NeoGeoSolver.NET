using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class CircleDiameter : Value
{
  private readonly Circle _circle;

  public CircleDiameter(Circle circle)
  {
    _circle = circle;
    Satisfy();
  }

  private Expression RadiusExpr
  {
    get
    {
      return _circle.RadiusExpr();
    }
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return RadiusExpr * 2.0 - value.Expr;
    }
  }
}
