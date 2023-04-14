using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointArcDistance : Value
{
  private readonly Point _point;
  private readonly Arc _arc;

  public PointArcDistance(Point pt, Arc circle)
  {
    _point = pt;
    _arc = circle;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var pPos = _point.Expr;
      var cCen = _arc.Centre.Expr;
      var cRad = _arc.Radius.Expr;

      yield return (pPos - cCen).Magnitude() - cRad - value.Expr;
    }
  }

  public override IEnumerable<Entity> Entities
  {
    get
    {
      yield return _point;
      yield return _arc;
    }
  }
}
