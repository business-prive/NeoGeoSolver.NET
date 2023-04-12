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

  public Expression LengthExpr()
  {
    return (Point1.Expr - Point0.Expr).Magnitude();
  }
}
