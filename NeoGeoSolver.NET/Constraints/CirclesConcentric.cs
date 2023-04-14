using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class CirclesConcentric : Constraint
{
  private readonly Circle _circle0;
  private readonly Circle _circle1;
  private readonly PointsDistance _dist_C0_C1;

  public CirclesConcentric(Circle circle0, Circle circle1)
  {
    _circle0 = circle0;
    _circle1 = circle1;

    _dist_C0_C1 = new PointsDistance(_circle0.Centre, _circle1.Centre);
    _dist_C0_C1.SetValue(0);
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      foreach (var expr in _dist_C0_C1.Equations)
      {
        yield return expr;
      }
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
