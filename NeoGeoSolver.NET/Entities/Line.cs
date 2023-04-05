using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Line : Entity
{
  public Point Point0 = new();
  public Point Point1 = new();

  public Line(Point p0, Point p1)
  {
    Point0 = p0;
    Point1 = p1;
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
    var pt0 = Point0.exp;
    var pt1 = Point1.exp;
    return pt0 + (pt1 - pt0) * t;
  }

  public override ExpressionVector TangentAt(Expression t)
  {
    return Point1.exp - Point0.exp;
  }

  public Expression LengthExpr()
  {
    return (Point1.exp - Point0.exp).Magnitude();
  }
}
