using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointOnArcMidpoint : Constraint
{
  private readonly Point _point;
  private readonly Arc _arc;

  public PointOnArcMidpoint(Point point, Arc arc)
  {
    _point = point;
    _arc = arc;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var midAngle = (_arc.EndAngle.Expr - _arc.StartAngle.Expr) / 2;
      var midPtX = _arc.Centre.X.Expr + _arc.Radius.Expr * Expression.Cos(midAngle);
      var midPtY = _arc.Centre.Y.Expr + _arc.Radius.Expr * Expression.Sin(midAngle);
      var midPtZ = Expression.Zero;
      var midPt = new ExpressionVector(midPtX, midPtY, midPtZ);
      var dist = (_point.Expr - midPt).Magnitude();
      yield return dist;
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
