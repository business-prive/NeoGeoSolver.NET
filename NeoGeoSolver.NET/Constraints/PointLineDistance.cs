using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointLineDistance : Value
{
  public Point Point { get; }

  public Line Line { get; }

  public PointLineDistance(Point pt, Line line)
  {
    Point = pt;
    Line = line;
    SetValue(1.0);
    Satisfy();
  }

  public override IEnumerable<Expression> equations
  {
    get
    {
      yield return ConstraintExp.PointLineDistance(Point.exp, Line.p0.exp, Line.p1.exp) - value;
    }
  }
}
