using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsHorizontalVertical : Constraint
{
  public readonly HorizontalVerticalOrientation orientation = HorizontalVerticalOrientation.Ox;

  private readonly Point _p0;
  private readonly Point _p1;

  public PointsHorizontalVertical(Point p0, Point p1)
  {
    _p0 = p0;
    _p1 = p1;
  }

  private readonly Line _line;

  public PointsHorizontalVertical(Line line)
  {
    _line = line;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      switch (orientation)
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
