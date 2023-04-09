using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class CirclesConcentric : Constraint
{
  private readonly Circle _c0;
  private readonly Circle _c1;
  private readonly PointsDistance _dist_C0_C1;

  public CirclesConcentric(Circle c0, Circle c1)
  {
    _c0 = c0;
    _c1 = c1;

    _dist_C0_C1 = new PointsDistance(_c0.Centre, _c1.Centre);
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
}
