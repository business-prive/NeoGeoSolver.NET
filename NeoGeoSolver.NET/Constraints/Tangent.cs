using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Tangent : Constraint
{
  public enum Option
  {
    Codirected,
    Antidirected
  }

  private Param _t0 = new("t0");
  private Param _t1 = new("t1");

  private readonly Line _l0;
  private readonly Line _l1;

  public Tangent(Line l0, Line l1)
  {
    _l0 = l0;
    _l1 = l1;
    Satisfy();
    ChooseBestOption();
  }

  public Option option { get; set; }

  protected override Enum optionInternal
  {
    get
    {
      return option;
    }
    set
    {
      option = (Option) value;
    }
  }

  public override IEnumerable<Param> Parameters
  {
    get
    {
      var tv0 = 0.0;
      var tv1 = 0.0;
      Expression c = null;
      Param p = null;
      if (!IsCoincident(ref tv0, ref tv1, ref c, ref p))
      {
        yield return _t0;
        yield return _t1;
      }
      else
      {
        if (p != null) yield return p;
      }
    }
  }

  private bool Satisfy()
  {
    var sys = new EquationSystem();
    sys.AddParameters(Parameters);
    _addAngle = false;
    var exprs = Equations.ToList();
    _addAngle = true;
    sys.AddEquations(Equations);

    var bestI = 0.0;
    var bestJ = 0.0;
    var min = -1.0;
    for (var i = 0.0; i < 1.0; i += 0.25 / 2.0)
    {
      for (var j = 0.0; j < 1.0; j += 0.25 / 2.0)
      {
        _t0.Value = i;
        _t1.Value = j;
        sys.Solve();
        var curValue = exprs.Sum(e => Math.Abs(e.Eval()));
        if (min >= 0.0 && min < curValue) continue;
        bestI = _t0.Value;
        bestJ = _t1.Value;
        min = curValue;
      }
    }

    _t0.Value = bestI;
    _t1.Value = bestJ;
    return true;
  }

  private bool IsCoincident(ref double tv0, ref double tv1, ref Expression c, ref Param p)
  {
    var l0 = _l0;
    var l1 = _l1;
    var s0 = l0;
    var s1 = l1;
    if (s0 != null && s1 != null)
    {
      if (s0.Point0.IsCoincidentWith(s1.Point0))
      {
        tv0 = 0.0;
        tv1 = 0.0;
        return true;
      }

      if (s0.Point0.IsCoincidentWith(s1.Point1))
      {
        tv0 = 0.0;
        tv1 = 1.0;
        return true;
      }

      if (s0.Point1.IsCoincidentWith(s1.Point0))
      {
        tv0 = 1.0;
        tv1 = 0.0;
        return true;
      }

      if (s0.Point1.IsCoincidentWith(s1.Point1))
      {
        tv0 = 1.0;
        tv1 = 1.0;
        return true;
      }
    }

    if (s0 != null)
    {
#if false
      PointOn pOn = null;
      if (s0.Point0.IsCoincidentWithCurve(l1, ref pOn))
      {
        tv0 = 0.0;
        p = _t1;
        c = new Expression(_t1) - pOn.GetValueParam();
        return true;
      }

      if (s0.Point1.IsCoincidentWithCurve(l1, ref pOn))
      {
        tv0 = 1.0;
        p = _t1;
        c = new Expression(_t1) - pOn.GetValueParam();
        return true;
      }
#endif
    }

    if (s1 != null)
    {
#if false
      PointOn pOn = null;
      if (s1.Point0.IsCoincidentWithCurve(l0, ref pOn))
      {
        p = _t0;
        c = new Expression(_t0) - pOn.GetValueParam();
        tv1 = 0.0;
        return true;
      }

      if (s1.Point1.IsCoincidentWithCurve(l0, ref pOn))
      {
        p = _t0;
        c = new Expression(_t0) - pOn.GetValueParam();
        tv1 = 1.0;
        return true;
      }
#endif
    }

    return false;
  }

  private bool _addAngle = true;

  public override IEnumerable<Expression> Equations
  {
    get
    {
#if false
      var l0 = _l0;
      var l1 = _l1;

      var dir0 = l0.TangentAt(_t0);
      var dir1 = l1.TangentAt(_t1);

      dir0 = l0.plane.DirFromPlane(dir0);
      dir0 = sketch.plane.DirToPlane(dir0);

      dir1 = l1.plane.DirFromPlane(dir1);
      dir1 = sketch.plane.DirToPlane(dir1);

      if (_addAngle)
      {
        var angle = ConstraintExp.Angle2d(dir0, dir1);
        switch (option)
        {
          case Option.Codirected:
            yield return angle;
            break;
          case Option.Antidirected:
            yield return Expression.Abs(angle) - Math.PI;
            break;
        }
      }

      var tv0 = _t0.Value;
      var tv1 = _t1.Value;
      Expression c = null;
      Param p = null;
      if (IsCoincident(ref tv0, ref tv1, ref c, ref p))
      {
        _t0.Value = tv0;
        _t1.Value = tv1;
        if (c != null)
        {
          yield return c;
        }
      }
      else
      {
        var eq = l1.PointOnInPlane(_t1) - l0.PointOnInPlane(_t0);

        yield return eq.x;
        yield return eq.y;
      }
#endif
      return Enumerable.Empty<Expression>();
    }
  }
}
