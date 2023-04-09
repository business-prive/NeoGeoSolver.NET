using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class ArcCircleConcentric : Constraint
{
  private readonly Arc _a0;
  private readonly Circle _c1;
  private readonly PointsDistance _dist_A0_C1;

  public ArcCircleConcentric(Arc a0, Circle c1)
  {
    _a0 = a0;
    _c1 = c1;

    _dist_A0_C1 = new PointsDistance(_a0.Centre, _c1.Centre);
    _dist_A0_C1.SetValue(0);
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      foreach (var expr in _dist_A0_C1.Equations)
      {
        yield return expr;
      }
    }
  }
}
