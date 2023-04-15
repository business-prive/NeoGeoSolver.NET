using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointOnCircleQuadConstraint : Constraint
{
  private readonly Point _point;
  private readonly Circle _circle;
  private readonly int _quadIndex;

  public PointOnCircleQuadConstraint(Point point, Circle circle, int quadIndex)
  {
    _point = point;
    _circle = circle;
    _quadIndex = quadIndex;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var c1CenterX = _circle.Centre.X.Expr;
      var c1CenterY = _circle.Centre.Y.Expr;
      var ex = c1CenterX;
      var ey = c1CenterY;
      var c1Rad = _circle.Radius.Expr;
      switch (_quadIndex)
      {
        // East
        case 0:
          ex += c1Rad;
          break;

        // North
        case 1:
          ey += c1Rad;
          break;

        // West
        case 2:
          ex -= c1Rad;
          break;

        // South
        case 3:
          ey -= c1Rad;
          break;

        default:
          throw new ArgumentOutOfRangeException();
      }

      var p1X = _point.X.Expr;
      var p1Y = _point.Y.Expr;
      var tempX = ex - p1X;
      var tempY = ey - p1Y;
      yield return tempX * tempX + tempY * tempY;
    }
  }

  public override IEnumerable<Entity> Entities
  {
    get
    {
     yield return _point;
     yield return _circle;
    }
  }
}
