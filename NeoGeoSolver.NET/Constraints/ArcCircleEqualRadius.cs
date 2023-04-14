using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class ArcCircleEqualRadius : Constraint
{
  private readonly Arc _arc;
  private readonly Circle _circle;

  public ArcCircleEqualRadius(Arc arc, Circle circle)
  {
    _arc = arc;
    _circle = circle;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return _arc.Radius.Expr - _circle.Radius.Expr;
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
