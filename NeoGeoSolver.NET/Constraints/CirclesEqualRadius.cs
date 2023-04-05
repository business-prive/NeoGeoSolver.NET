using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class CirclesEqualRadius : Value
{
  private readonly Circle _c0;
  private readonly Circle _c1;

  public CirclesEqualRadius(Circle c0, Circle c1)
  {
    _c0 = c0;
    _c1 = c1;
    value.value = 1.0;
  }

  public override IEnumerable<Expression> equations
  {
    get
    {
      yield return _c0.Radius() - _c1.Radius() * value;
    }
  }
}
