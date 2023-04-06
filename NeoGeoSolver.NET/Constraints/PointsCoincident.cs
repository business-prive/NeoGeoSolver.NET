using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsCoincident : Constraint
{
  private readonly Point _p0;
  private readonly Point _p1;

  public PointsCoincident(Point pt0, Point pt1)
  {
    _p0 = pt0;
    _p1 = pt1;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var pe0 = _p0.Expr;
      var pe1 = _p1.Expr;
      yield return pe0.x - pe1.x;
      yield return pe0.y - pe1.y;
    }
  }
}
