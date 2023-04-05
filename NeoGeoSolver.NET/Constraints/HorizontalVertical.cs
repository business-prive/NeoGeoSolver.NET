using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class HorizontalVertical : Constraint
{
  private ExpressionVector p0Exp
  {
    get
    {
      return _p0.exp;
    }
  }

  private ExpressionVector p1Exp
  {
    get
    {
      return _p1.exp;
    }
  }

  public HorizontalVerticalOrientation orientation = HorizontalVerticalOrientation.Ox;

  private readonly Point _p0;
  private readonly Point _p1;

  public HorizontalVertical(Point p0, Point p1)
  {
    _p0 = p0;
    _p1 = p1;
  }

  private readonly Line _line;

  public HorizontalVertical(Line line)
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
          yield return p0Exp.x - p1Exp.x;
          break;
        case HorizontalVerticalOrientation.Oy:
          yield return p0Exp.y - p1Exp.y;
          break;
        case HorizontalVerticalOrientation.Oz:
          yield return p0Exp.z - p1Exp.z;
          break;
      }
    }
  }
}
