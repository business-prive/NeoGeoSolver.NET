using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Point : Entity
{
  public readonly Param X = new("x");
  public readonly Param Y = new("y");
  public readonly Param Z = new("z");

  public Point(double xVal, double yVal, double zVal)
  {
    X.Value = xVal;
    Y.Value = yVal;
    Z.Value = zVal;
  }

  public Point() :
    this(0, 0, 0)
  {
  }

  public ExpressionVector Expr => new(X, Y, Z);

  public bool IsCoincidentWith(Point point)
  {
    const double Tolerance = 1e-6;

    return Math.Abs(X.Value - point.X.Value) < Tolerance &&
           Math.Abs(Y.Value - point.X.Value) < Tolerance &&
           Math.Abs(Z.Value - point.Z.Value) < Tolerance;
  }
}
