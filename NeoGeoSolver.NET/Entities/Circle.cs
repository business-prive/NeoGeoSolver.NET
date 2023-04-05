using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Circle : Entity
{
  public Point Centre = new();
  public Param Radius = new("r");

  public override IEnumerable<Expression> equations
  {
    get
    {
      yield break;
    }
  }

  public override IEnumerable<Param> parameters
  {
    get
    {
      yield return Radius;
    }
  }

  public override ExpressionVector PointOn(Expression t)
  {
    var angle = t * 2.0 * Math.PI;
    return Centre.exp + new ExpressionVector(Expression.Cos(angle), Expression.Sin(angle), 0.0) * RadiusExpr();
  }

  public override ExpressionVector TangentAt(Expression t)
  {
    var angle = t * 2.0 * Math.PI;
    return new ExpressionVector(-Expression.Sin(angle), Expression.Cos(angle), 0.0);
  }

  public Expression LengthExpr()
  {
    return new Expression(2.0) * Math.PI * RadiusExpr();
  }

  public Expression RadiusExpr()
  {
    return Expression.Abs(Radius);
  }

  public ExpressionVector CentreExpr()
  {
    return Centre.exp;
  }
}
