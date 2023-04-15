using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsDistanceHorizontal : Value
{
  private readonly Point _point0;
  private readonly Point _point1;

  public PointsDistanceHorizontal(Point pt0, Point pt1)
  {
    _point0 = pt0;
    _point1 = pt1;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return _point1.X.Expr - _point0.X.Expr - value.Expr;
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