using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointOn : Value
{
  public Point point
  {
    get
    {
      return (Point) GetEntity(0);
    }
    set
    {
      SetEntity(0, value);
    }
  }

  public IEntity on
  {
    get
    {
      return GetEntity(1);
    }
    set
    {
      SetEntity(1, value);
    }
  }

  public ExpressionVector pointExp
  {
    get
    {
      return point.exp;
    }
  }

  protected override bool OnSatisfy()
  {
    var sys = new EquationSystem();
    sys.AddParameters(Parameters);
    var exprs = Equations.ToList();
    sys.AddEquations(Equations);

    var bestI = 0.0;
    var min = -1.0;
    for (var i = 0.0; i < 1.0; i += 0.25 / 2.0)
    {
      value.Value = i;
      sys.Solve();
      var curValue = exprs.Sum(e => Math.Abs(e.Eval()));
      if (min >= 0.0 && min < curValue) continue;
      bestI = value.Value;
      min = curValue;
    }

    value.Value = bestI;
    return true;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var p = pointExp;
      var eq = on.PointOnInPlane(value) - p;

      yield return eq.x;
      yield return eq.y;
    }
  }
}
