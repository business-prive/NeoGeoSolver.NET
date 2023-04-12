using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Arc : Entity
{
  public readonly Point Centre = new();
  public readonly Point Point0 = new();
  public readonly Point Point1 = new();

  public Arc(Point centre, Point pt0, Point pt1)
  {
    Centre = centre;
    Point0 = pt0;
    Point1 = pt1;
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
}
