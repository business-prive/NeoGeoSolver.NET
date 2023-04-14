using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsHorizontalVertical : Constraint
{
  public readonly HorizontalVerticalOrientation Orientation = HorizontalVerticalOrientation.Ox;

  private readonly Point _point0;
  private readonly Point _point1;

  public PointsHorizontalVertical(Point point0, Point point1, HorizontalVerticalOrientation orientation)
  {
    _point0 = point0;
    _point1 = point1;
     Orientation = orientation;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      switch (Orientation)
      {
        case HorizontalVerticalOrientation.Ox:
          yield return _point0.Expr.x - _point1.Expr.x;
          break;
        case HorizontalVerticalOrientation.Oy:
          yield return _point0.Expr.y - _point1.Expr.y;
          break;
        case HorizontalVerticalOrientation.Oz:
          yield return _point0.Expr.z - _point1.Expr.z;
          break;
      }
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
