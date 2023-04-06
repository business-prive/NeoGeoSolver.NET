using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class LineHorizontalVertical : Constraint
{
  public readonly HorizontalVerticalOrientation orientation = HorizontalVerticalOrientation.Ox;

  private readonly Line _line;

  public LineHorizontalVertical(Line line)
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
          yield return _line.Point0.exp.x - _line.Point1.exp.x;
          break;
        case HorizontalVerticalOrientation.Oy:
          yield return _line.Point0.exp.y - _line.Point1.exp.y;
          break;
        case HorizontalVerticalOrientation.Oz:
          yield return _line.Point0.exp.z - _line.Point1.exp.z;
          break;
      }
    }
  }
}
