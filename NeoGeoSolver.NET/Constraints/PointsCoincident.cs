using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsCoincident : Constraint
{
  private readonly Point _point0;
  private readonly Point _point1;

  public PointsCoincident(Point pt0, Point pt1)
  {
    _point0 = pt0;
    _point1 = pt1;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var pe0 = _point0.Expr;
      var pe1 = _point1.Expr;
      yield return pe0.x - pe1.x;
      yield return pe0.y - pe1.y;
    }
  }

  public override IEnumerable<Entity> Entities
  {
    get
    {
      yield return _point0;
      yield return _point1;
    }
  }
}
