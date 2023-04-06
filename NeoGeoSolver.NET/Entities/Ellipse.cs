using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Ellipse : Entity
{
  public readonly Point Centre = new();
  public readonly Param Radius0 = new("r0");
  public readonly Param Radius1 = new("r1");
  private readonly ExpressionBasis2d _basis = new();

  public Ellipse(Point centre, Param rad0, Param rad1)
  {
    Centre = centre;
    Radius0 = rad0;
    Radius1 = rad1;
    _basis.SetPosParams(Centre.x, Centre.y);
  }

  public override IEnumerable<Param> Parameters
  {
    get
    {
      yield return Radius0;
      yield return Radius1;
      foreach (var p in _basis.Parameters)
      {
        yield return p;
      }
    }
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      foreach (var e in _basis.equations)
      {
        yield return e;
      }
    }
  }

  public ExpressionVector CentreExpr()
  {
    return Centre.Expr;
  }

  public override ExpressionVector PointOn(Expression t)
  {
    var angle = t * 2.0 * Math.PI;
    return _basis.TransformPosition(new ExpressionVector(Expression.Cos(angle) * Expression.Abs(Radius0), Expression.Sin(angle) * Expression.Abs(Radius1), 0.0));
  }
}
