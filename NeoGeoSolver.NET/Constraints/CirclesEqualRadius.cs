using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class CirclesEqualRadius : Value
{
  private readonly Circle _circle0;
  private readonly Circle _circle1;

  public CirclesEqualRadius(Circle circle0, Circle circle1)
  {
    _circle0 = circle0;
    _circle1 = circle1;
    value.Value = 1.0;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return _circle0.RadiusExpr() - _circle1.RadiusExpr() * value;
    }
  }

  public override IEnumerable<Entity> Entities
  {
    get
    {
      yield return _circle0;
      yield return _circle1;
    }
  }
}
