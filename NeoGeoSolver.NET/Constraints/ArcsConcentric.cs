using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class ArcsConcentric : Constraint
{
  private readonly Arc _arc0;
  private readonly Arc _arc1;
  private readonly PointsDistance _dist_C0_C1;

  public ArcsConcentric(Arc arc0, Arc arc1)
  {
    _arc0 = arc0;
    _arc1 = arc1;

    _dist_C0_C1 = new PointsDistance(_arc0.Centre, _arc1.Centre);
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
      yield return _arc0;
      yield return _arc1;
    }
  }
}
