using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class ArcRadius : Value
{
  private readonly Arc _arc;

  public ArcRadius(Arc arc)
  {
    _arc = arc;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return _arc.Radius.Expr - value.Value;
    }
  }
}
