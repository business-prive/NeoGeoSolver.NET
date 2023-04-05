using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Point : Entity
{
  public Param x = new("x");
  public Param y = new("y");
  public Param z = new("z");

  public Point(double xVal, double yVal, double zVal)
  {
    x.value = xVal;
    y.value = yVal;
    z.value = zVal;
  }

  public Point() :
    this(0, 0, 0)
  {
  }
  public override IEnumerable<Expression> equations
  {
    get
    {
      yield break;
    }
  }

  public ExpressionVector exp => new(x, y, z);

  public override IEnumerable<Param> parameters
  {
    get
    {
      yield return x;
      yield return y;
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
    return exp;
  }
}
