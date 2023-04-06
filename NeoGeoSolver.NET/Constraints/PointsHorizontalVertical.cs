using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsHorizontalVertical : Constraint
{
  public readonly HorizontalVerticalOrientation Orientation = HorizontalVerticalOrientation.Ox;

  private readonly Point _p0;
  private readonly Point _p1;

  public PointsHorizontalVertical(Point p0, Point p1, HorizontalVerticalOrientation orientation)
  {
    _p0 = p0;
    _p1 = p1;
     Orientation = orientation;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      switch (Orientation)
      {
        case HorizontalVerticalOrientation.Ox:
          yield return _p0.exp.x - _p1.exp.x;
          break;
        case HorizontalVerticalOrientation.Oy:
          yield return _p0.exp.y - _p1.exp.y;
          break;
        case HorizontalVerticalOrientation.Oz:
          yield return _p0.exp.z - _p1.exp.z;
          break;
      }
    }
  }
}
