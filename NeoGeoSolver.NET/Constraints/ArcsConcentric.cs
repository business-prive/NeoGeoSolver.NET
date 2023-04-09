using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class ArcsConcentric : Constraint
{
  private readonly Arc _a0;
  private readonly Arc _a1;
  private readonly PointsDistance _dist_C0_C1;

  public ArcsConcentric(Arc a0, Arc a1)
  {
    _a0 = a0;
    _a1 = a1;

    _dist_C0_C1 = new PointsDistance(_a0.Centre, _a1.Centre);
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
