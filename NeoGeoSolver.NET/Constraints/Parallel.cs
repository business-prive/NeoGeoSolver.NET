using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Parallel : Constraint
{
  public enum Option
  {
    Codirected,
    Antidirected
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

  private readonly Line _l0;
  private readonly Line _l1;

  public Parallel(Line l0, Line l1)
  {
    _l0 = l0;
    _l1 = l1;
    ChooseBestOption();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var d0 = _l0.Point0.Expr - _l0.Point1.Expr;
      var d1 = _l1.Point0.Expr - _l1.Point1.Expr;

      var angle = ConstraintExp.Angle2d(d0, d1);
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
  }
}
