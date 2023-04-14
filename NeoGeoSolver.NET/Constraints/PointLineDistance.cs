using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointLineDistance : Value
{
  private readonly Point _point;
  private readonly Line _line;

  public PointLineDistance(Point pt, Line line)
  {
    _point = pt;
    _line = line;
    SetValue(1.0);
    Satisfy();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return ConstraintExp.PointLineDistance(_point.Expr, _line.Point0.Expr, _line.Point1.Expr) - value;
    }
  }

  public override IEnumerable<Entity> Entities
  {
    get
    {
      yield return _point;
      yield return _line;
    }
  }
}
