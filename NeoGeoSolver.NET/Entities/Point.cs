using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Point : Entity
{
  public readonly Param x = new("x");
  public readonly Param y = new("y");
  public readonly Param z = new("z");

  public Point(double xVal, double yVal, double zVal)
  {
    x.Value = xVal;
    y.Value = yVal;
    z.Value = zVal;
  }

  public Point() :
    this(0, 0, 0)
  {
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield break;
    }
  }

  public ExpressionVector Expr => new(x, y, z);

  public override IEnumerable<Param> Parameters
  {
    get
    {
      yield return x;
      yield return y;
      yield return z;
    }
  }

  public bool IsCoincidentWith(Point point)
  {
    if (IsSameAs(point, this))
    {
      return true;
    }

    return false;
  }

  private static bool IsSameAs(IEntity e0, IEntity e1)
  {
    if (e0 == null)
    {
      return e1 == null;
    }

    if (e1 == null)
    {
      return e0 == null;
    }

    return e0 == e1 || e0.GetType() == e1.GetType();
  }

  public override ExpressionVector PointOn(Expression t)
  {
    return Expr;
  }
}
