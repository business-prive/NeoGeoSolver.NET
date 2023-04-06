using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsDistance : Value
{
  private readonly Point _p0;
  private readonly Point _p1;

  public PointsDistance(Point pt0, Point pt1)
  {
    _p0 = pt0;
    _p1 = pt1;
    Satisfy();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return (_p1.Expr - _p0.Expr).Magnitude() - value.Expr;
    }
  }
}
