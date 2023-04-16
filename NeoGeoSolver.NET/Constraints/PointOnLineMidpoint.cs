using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointOnLineMidpoint : Constraint
{
  private readonly Point _point;
  private readonly Line _line;

  public PointOnLineMidpoint(Point point, Line line)
  {
    _point = point;
    _line = line;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var midPt = (_line.Point1.Expr - _line.Point0.Expr) / 2;
      var dist = (_point.Expr - midPt).Magnitude();
      yield return dist;
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
