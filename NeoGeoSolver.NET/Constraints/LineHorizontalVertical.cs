using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class LineHorizontalVertical : Constraint
{
  public readonly HorizontalVerticalOrientation Orientation = HorizontalVerticalOrientation.Ox;

  private readonly Line _line;

  public LineHorizontalVertical(Line line, HorizontalVerticalOrientation orientation)
  {
    _line = line;
     Orientation = orientation;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      switch (Orientation)
      {
        case HorizontalVerticalOrientation.Ox:
          yield return _line.Point0.Expr.x - _line.Point1.Expr.x;
          break;
        case HorizontalVerticalOrientation.Oy:
          yield return _line.Point0.Expr.y - _line.Point1.Expr.y;
          break;
        case HorizontalVerticalOrientation.Oz:
          yield return _line.Point0.Expr.z - _line.Point1.Expr.z;
          break;
      }
    }
  }
}
