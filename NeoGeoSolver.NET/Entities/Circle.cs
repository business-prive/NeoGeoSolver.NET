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

  public Expression RadiusExpr()
  {
    return Expression.Abs(Radius);
  }

  public ExpressionVector CentreExpr()
  {
    return Centre.Expr;
  }
}
