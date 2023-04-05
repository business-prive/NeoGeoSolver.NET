using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class CirclesDistance : Value
{
  public enum Option
  {
    Outside,
    FirstInside,
    SecondInside,
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

  private readonly Circle _c0;
  private readonly Circle _c1;

  public CirclesDistance(Circle c0, Circle c1)
  {
    _c0 = c0;
    _c1 = c1;
    value.Value = 1;
    ChooseBestOption();
    Satisfy();
  }

  private bool IsCentersCoincident(Circle c0, Circle c1)
  {
    var cp0 = c0.Centre;
    var cp1 = c1.Centre;
    return cp0 != null && cp1 != null && cp0.IsCoincidentWith(cp1);
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var c0 = _c0;
      var c1 = _c1;
      var r0 = c0.RadiusExpr();
      var r1 = c1.RadiusExpr();
      if (IsCentersCoincident(c0, c1))
      {
        if (option == Option.FirstInside)
        {
          yield return r0 - r1 - value.Expr;
        }
        else
        {
          yield return r1 - r0 - value.Expr;
        }
      }
      else
      {
        var dist = (c0.CentreExpr() - c1.CentreExpr()).Magnitude();
        switch (option)
        {
          case Option.Outside:
            yield return (dist - r0 - r1) - value.Expr;
            break;
          case Option.FirstInside:
            yield return (r1 - r0 - dist) - value.Expr;
            break;
          case Option.SecondInside:
            yield return (r0 - r1 - dist) - value.Expr;
            break;
        }
      }
    }
  }
}
