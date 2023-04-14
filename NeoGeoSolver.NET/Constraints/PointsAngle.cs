using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Constraints;

public class PointsAngle : Value
{
  private bool _supplementary;

  public bool Supplementary
  {
    get
    {
      return _supplementary;
    }
    set
    {
      if (value == _supplementary)
      {
        return;
      }

      Supplementary = value;
      this.value.Value = 2.0 * Math.PI - this.value.Value;
    }
  }

  private readonly Point[] _points = new Point[4];

  public PointsAngle(Point[] points)
  {
    if (points.Length != 4)
    {
      throw new ArgumentOutOfRangeException();
    }

    _points[0] = points[0];
    _points[1] = points[1];
    _points[2] = points[2];
    _points[3] = points[3];
    Satisfy();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var p = GetPointsExp();
      var d0 = p[0] - p[1];
      var d1 = p[3] - p[2];
      var angle = ConstraintExp.Angle2d(d0, d1);
      yield return angle - value;
    }
  }

  public override IEnumerable<Entity> Entities
  {
    get
    {
      yield return _points[0];
      yield return _points[1];
      yield return _points[2];
      yield return _points[3];
    }
  }

  private ExpressionVector[] GetPointsExp()
  {
    var p = new ExpressionVector[4];
    for (var i = 0; i < 4; i++)
    {
      p[i] = _points[i].Expr;
    }

    if (Supplementary)
    {
      SystemExt.Swap(ref p[2], ref p[3]);
    }

    return p;
  }
}
