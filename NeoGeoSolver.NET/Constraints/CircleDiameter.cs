using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class CircleDiameter : Value
{
  private readonly Circle _circle;

  public CircleDiameter(Circle circle)
  {
    circle = circle;
    Satisfy();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return _circle.RadiusExpr() * 2.0 - value.Expr;
    }
  }
}
