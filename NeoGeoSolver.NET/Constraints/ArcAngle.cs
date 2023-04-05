using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Constraints;

public class ArcAngle : Value
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

  private readonly Arc _arc;

  public ArcAngle(Arc arc)
  {
    _arc = arc;
    value.Value = Math.PI / 4;
    Satisfy();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var p = GetPointsExp();
      var d0 = p[0] - p[1];
      var d1 = p[3] - p[2];
      var angle = ConstraintExp.Angle2d(d0, d1, true);
      yield return angle - value;
    }
  }

  private ExpressionVector[] GetPointsExp()
  {
    var p = new ExpressionVector[4];
    var arc = _arc;
    p[0] = arc.Point0.exp;
    p[1] = arc.Centre.exp;
    p[2] = arc.Centre.exp;
    p[3] = arc.Point1.exp;
    if (Supplementary)
    {
      SystemExt.Swap(ref p[0], ref p[3]);
      SystemExt.Swap(ref p[1], ref p[2]);
    }

    return p;
  }
}
