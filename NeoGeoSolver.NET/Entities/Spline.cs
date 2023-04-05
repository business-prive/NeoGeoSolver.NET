using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Spline : Entity
{
  public readonly Point[] Points = new Point[4];

  public Spline(Point[] points)
  {
    if (points.Length != 4)
    {
      throw new ArgumentOutOfRangeException();
    }

    Points[0] = points[0];
    Points[1] = points[1];
    Points[2] = points[2];
    Points[3] = points[3];
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield break;
    }
  }

  public override ExpressionVector PointOn(Expression t)
  {
    var p0 = Points[0].exp;
    var p1 = Points[1].exp;
    var p2 = Points[2].exp;
    var p3 = Points[3].exp;
    var t2 = t * t;
    var t3 = t2 * t;
    return p1 * (3.0 * t3 - 6.0 * t2 + 3.0 * t) + p3 * t3 + p2 * (3.0 * t2 - 3.0 * t3) - p0 * (t3 - 3.0 * t2 + 3.0 * t - 1.0);
  }
}
