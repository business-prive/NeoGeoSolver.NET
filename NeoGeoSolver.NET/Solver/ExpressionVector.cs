using System.Numerics;

namespace NeoGeoSolver.NET.Solver;

public class ExpressionVector
{
  public Expression x;
  public Expression y;
  public readonly Expression z;

  public ExpressionVector(Expression xExpr, Expression yExpr, Expression zExpr)
  {
    x = xExpr;
    y = yExpr;
    z = zExpr;
  }

  public static implicit operator ExpressionVector(Vector3 v)
  {
    return new ExpressionVector(v.X, v.Y, v.Z);
  }

  public static ExpressionVector operator +(ExpressionVector a, ExpressionVector b)
  {
    return new ExpressionVector(a.x + b.x, a.y + b.y, a.z + b.z);
  }

  public static ExpressionVector operator -(ExpressionVector a, ExpressionVector b)
  {
    return new ExpressionVector(a.x - b.x, a.y - b.y, a.z - b.z);
  }

  public static ExpressionVector operator *(ExpressionVector a, ExpressionVector b)
  {
    return new ExpressionVector(a.x * b.x, a.y * b.y, a.z * b.z);
  }

  public static ExpressionVector operator /(ExpressionVector a, ExpressionVector b)
  {
    return new ExpressionVector(a.x / b.x, a.y / b.y, a.z / b.z);
  }

  public static ExpressionVector operator -(ExpressionVector b)
  {
    return new ExpressionVector(-b.x, -b.y, -b.z);
  }

  public static ExpressionVector operator *(Expression a, ExpressionVector b)
  {
    return new ExpressionVector(a * b.x, a * b.y, a * b.z);
  }

  public static ExpressionVector operator *(ExpressionVector a, Expression b)
  {
    return new ExpressionVector(a.x * b, a.y * b, a.z * b);
  }

  public static ExpressionVector operator /(Expression a, ExpressionVector b)
  {
    return new ExpressionVector(a / b.x, a / b.y, a / b.z);
  }

  public static ExpressionVector operator /(ExpressionVector a, Expression b)
  {
    return new ExpressionVector(a.x / b, a.y / b, a.z / b);
  }

  public static Expression Dot(ExpressionVector a, ExpressionVector b)
  {
    return a.x * b.x + a.y * b.y + a.z * b.z;
  }

  public static ExpressionVector Cross(ExpressionVector a, ExpressionVector b)
  {
    return new ExpressionVector(
      a.y * b.z - b.y * a.z,
      a.z * b.x - b.z * a.x,
      a.x * b.y - b.x * a.y
    );
  }

  public Expression Magnitude()
  {
    return Expression.Sqrt(Expression.Sqr(x) + Expression.Sqr(y) + Expression.Sqr(z));
  }

  public Vector3 Eval()
  {
    return new Vector3((float) x.Eval(), (float) y.Eval(), (float) z.Eval());
  }
}
