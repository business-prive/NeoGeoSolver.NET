using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsDistance : Value
{
  private readonly Point _point0;
  private readonly Point _point1;

  public PointsDistance(Point pt0, Point pt1)
  {
    _point0 = pt0;
    _point1 = pt1;
    Satisfy();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return (_point1.Expr - _point0.Expr).Magnitude() - value.Expr;
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
