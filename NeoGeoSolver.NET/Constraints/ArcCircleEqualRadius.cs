using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class ArcCircleEqualRadius : Constraint
{
  private readonly Arc _a0;
  private readonly Circle _c1;

  public ArcCircleEqualRadius(Arc a0, Circle c1)
  {
    _a0 = a0;
    _c1 = c1;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return _a0.Radius.Expr - _c1.Radius.Expr;
    }
  }
}
