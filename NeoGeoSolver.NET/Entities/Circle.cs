using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Circle : Entity
{
  public readonly Point Centre = new();
  public readonly Param Radius = new("r");

  public Circle(Point centre, Param radius)
  {
    Centre = centre;
    Radius = radius;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield break;
    }
  }

  public override IEnumerable<Param> Parameters
  {
    get
    {
      yield return Radius;
    }
  }

  public Expression RadiusExpr()
  {
    return Expression.Abs(Radius);
  }

  public ExpressionVector CentreExpr()
  {
    return Centre.Expr;
  }
}
