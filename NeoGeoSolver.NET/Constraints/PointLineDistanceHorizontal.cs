using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointLineDistanceHorizontal : Value
{
  private readonly Point _point;
  private readonly Line _line;

  public PointLineDistanceHorizontal(Point pt, Line line)
  {
    _point = pt;
    _line = line;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var l1P1X = _line.Point0.X.Expr;
      var l1P1Y = _line.Point0.Y.Expr;
      var l1P2X = _line.Point1.X.Expr;
      var l1P2Y = _line.Point1.Y.Expr;
      var dx = l1P2X - l1P1X;
      var dy = l1P2Y - l1P1Y;

      var p1X = _point.X.Expr;
      var p1Y = _point.Y.Expr;
      var t = (p1Y - l1P1Y) / dy;
      var xint = l1P1X + dx * t;
      var distance = value.Expr;
      var temp = Expression.Abs(p1X - xint) - distance;
      yield return temp * temp / 10;
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
