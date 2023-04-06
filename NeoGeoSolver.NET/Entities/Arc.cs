using NeoGeoSolver.NET.Constraints;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Arc : Entity
{
  public readonly Point Point0 = new();
  public readonly Point Point1 = new();
  public readonly Point Centre = new();

  public Arc(Point pt0, Point pt1, Point centre)
  {
    Point0 = pt0;
    Point1 = pt1;
    Centre = centre;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      if (!Point0.IsCoincidentWith(Point1))
      {
        yield return (Point0.Expr - Centre.Expr).Magnitude() - (Point1.Expr - Centre.Expr).Magnitude();
      }
    }
  }

  private Expression GetAngleExp()
  {
    if (!Point0.IsCoincidentWith(Point1))
    {
      var d0 = Point0.Expr - Centre.Expr;
      var d1 = Point1.Expr - Centre.Expr;
      return ConstraintExp.Angle2d(d0, d1, angle360: true);
    }

    return Math.PI * 2.0;
  }

  public override ExpressionVector PointOn(Expression t)
  {
    var angle = GetAngleExp();
    var cos = Expression.Cos(angle * t);
    var sin = Expression.Sin(angle * t);
    var rv = Point0.Expr - Centre.Expr;

    return Centre.Expr + new ExpressionVector(
      cos * rv.x - sin * rv.y,
      sin * rv.x + cos * rv.y,
      0.0
    );
  }

  public override ExpressionVector TangentAt(Expression t)
  {
    var angle = GetAngleExp();
    var cos = Expression.Cos(angle * t + Math.PI / 2);
    var sin = Expression.Sin(angle * t + Math.PI / 2);
    var rv = Point0.Expr - Centre.Expr;

    return new ExpressionVector(
      cos * rv.x - sin * rv.y,
      sin * rv.x + cos * rv.y,
      0.0
    );
  }

  public Expression LengthExpr()
  {
    return GetAngleExp() * RadiusExpr();
  }

  public Expression RadiusExpr()
  {
    return (Point0.Expr - Centre.Expr).Magnitude();
  }

  public ExpressionVector CentreExpr()
  {
    return Centre.Expr;
  }
}
