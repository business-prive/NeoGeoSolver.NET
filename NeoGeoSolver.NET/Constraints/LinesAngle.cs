using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Constraints;

public class LinesAngle : Value
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
      this.value.Value = -(Math.Sign(this.value.Value) * Math.PI - this.value.Value);
    }
  }

  private readonly Line _line0;
  private readonly Line _line1;

  public LinesAngle(Line line0, Line line1)
  {
    _line0 = line0;
    _line1 = line1;
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
      yield return _line0;
      yield return _line1;
    }
  }

  private ExpressionVector[] GetPointsExp()
  {
    var p = new ExpressionVector[4];
    var l0 = _line0;
    p[0] = l0.Point0.Expr;
    p[1] = l0.Point1.Expr;
    var l1 = _line1;
    p[2] = l1.Point0.Expr;
    p[3] = l1.Point1.Expr;
    if (Supplementary)
    {
      SystemExt.Swap(ref p[2], ref p[3]);
    }

    return p;
  }
}
