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

  private Option option_;
  private Param t0 = new("t0");
  private Param t1 = new("t1");

  public Option option
  {
    get
    {
      return option_;
    }
    set
    {
      option_ = value;
    }
  }

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

  public override IEnumerable<Param> parameters
  {
    get
    {
      double tv0 = 0.0;
      double tv1 = 0.0;
      Expression c = null;
      Param p = null;
      if (!IsCoincident(ref tv0, ref tv1, ref c, ref p))
      {
        yield return t0;
        yield return t1;
      }
      else
      {
        if (p != null) yield return p;
      }
    }
  }

  public Tangent()
  {
  }

  public Tangent(IEntity l0, IEntity l1)
  {
    AddEntity(l0);
    AddEntity(l1);
    Satisfy();
    ChooseBestOption();
  }

  private bool Satisfy()
  {
    EquationSystem sys = new EquationSystem();
    sys.AddParameters(parameters);
    addAngle = false;
    var exprs = equations.ToList();
    addAngle = true;
    sys.AddEquations(equations);

    double bestI = 0.0;
    double bestJ = 0.0;
    double min = -1.0;
    for (double i = 0.0; i < 1.0; i += 0.25 / 2.0)
    {
      for (double j = 0.0; j < 1.0; j += 0.25 / 2.0)
      {
        t0.value = i;
        t1.value = j;
        sys.Solve();
        double cur_value = exprs.Sum(e => Math.Abs(e.Eval()));
        if (min >= 0.0 && min < cur_value) continue;
        bestI = t0.value;
        bestJ = t1.value;
        min = cur_value;
      }
    }

    t0.value = bestI;
    t1.value = bestJ;
    return true;
  }

  private bool IsCoincident(ref double tv0, ref double tv1, ref Expression c, ref Param p)
  {
    var l0 = GetEntity(0);
    var l1 = GetEntity(1);
    var s0 = l0;
    var s1 = l1;
    if (s0 != null && s1 != null)
    {
      if (s0.begin.IsCoincidentWith(s1.begin))
      {
        tv0 = 0.0;
        tv1 = 0.0;
        return true;
      }

      if (s0.begin.IsCoincidentWith(s1.end))
      {
        tv0 = 0.0;
        tv1 = 1.0;
        return true;
      }

      if (s0.end.IsCoincidentWith(s1.begin))
      {
        tv0 = 1.0;
        tv1 = 0.0;
        return true;
      }

      if (s0.end.IsCoincidentWith(s1.end))
      {
        tv0 = 1.0;
        tv1 = 1.0;
        return true;
      }
    }

    if (s0 != null)
    {
      PointOn pOn = null;
      if (s0.begin.IsCoincidentWithCurve(l1, ref pOn))
      {
        tv0 = 0.0;
        p = t1;
        c = new Expression(t1) - pOn.GetValueParam();
        return true;
      }

      if (s0.end.IsCoincidentWithCurve(l1, ref pOn))
      {
        tv0 = 1.0;
        p = t1;
        c = new Expression(t1) - pOn.GetValueParam();
        return true;
      }
    }

    if (s1 != null)
    {
      PointOn pOn = null;
      if (s1.begin.IsCoincidentWithCurve(l0, ref pOn))
      {
        p = t0;
        c = new Expression(t0) - pOn.GetValueParam();
        tv1 = 0.0;
        return true;
      }

      if (s1.end.IsCoincidentWithCurve(l0, ref pOn))
      {
        p = t0;
        c = new Expression(t0) - pOn.GetValueParam();
        tv1 = 1.0;
        return true;
      }
    }

    return false;
  }

  private bool addAngle = true;

  public override IEnumerable<Expression> equations
  {
    get
    {
      var l0 = GetEntity(0);
      var l1 = GetEntity(1);

      ExpressionVector dir0 = l0.TangentAt(t0);
      ExpressionVector dir1 = l1.TangentAt(t1);

      dir0 = l0.plane.DirFromPlane(dir0);
      dir0 = sketch.plane.DirToPlane(dir0);

      dir1 = l1.plane.DirFromPlane(dir1);
      dir1 = sketch.plane.DirToPlane(dir1);

      if (addAngle)
      {
        Expression angle = ConstraintExp.angle2d(dir0, dir1);
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

      double tv0 = t0.value;
      double tv1 = t1.value;
      Expression c = null;
      Param p = null;
      if (IsCoincident(ref tv0, ref tv1, ref c, ref p))
      {
        t0.value = tv0;
        t1.value = tv1;
        if (c != null) yield return c;
      }
      else
      {
        var eq = l1.PointOnInPlane(t1, sketch.plane) - l0.PointOnInPlane(t0, sketch.plane);

        yield return eq.x;
        yield return eq.y;
      }
    }
  }
}
