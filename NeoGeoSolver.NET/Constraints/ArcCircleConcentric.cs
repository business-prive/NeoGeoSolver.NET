using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class ArcCircleConcentric : Constraint
{
  private readonly Arc _arc;
  private readonly Circle _circle;
  private readonly PointsDistance _dist_A0_C1;

  public ArcCircleConcentric(Arc arc, Circle circle)
  {
    _arc = arc;
    _circle = circle;

    _dist_A0_C1 = new PointsDistance(_arc.Centre, _circle.Centre);
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

  public override IEnumerable<Entity> Entities
  {
    get
    {
      yield return _arc;
      yield return _circle;
    }
  }
}
