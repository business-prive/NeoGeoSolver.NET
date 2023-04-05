using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsCoincident : Constraint
{
  public Point Point0 { get; }
  public Point Point1 { get; }

  public PointsCoincident(Point pt0, Point pt1)
  {
    Point0 = pt0;
    Point1 = pt1;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var pe0 = Point0.exp;
      var pe1 = Point1.exp;
      yield return pe0.x - pe1.x;
      yield return pe0.y - pe1.y;
    }
  }
}
