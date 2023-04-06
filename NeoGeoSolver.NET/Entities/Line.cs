using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Line : Entity
{
  public readonly Point Point0 = new();
  public readonly Point Point1 = new();

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
    var pt0 = Point0.Expr;
    var pt1 = Point1.Expr;
    return pt0 + (pt1 - pt0) * t;
  }

  public override ExpressionVector TangentAt(Expression t)
  {
    return Point1.Expr - Point0.Expr;
  }

  public Expression LengthExpr()
  {
    return (Point1.Expr - Point0.Expr).Magnitude();
  }
}
