using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Perpendicular : Constraint
{
  public enum Option
  {
    LeftHand,
    RightHand
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

  public Perpendicular(Line l0, Line l1)
  {
    _l0 = l0;
    _l1 = l1;
    ChooseBestOption();
  }

  public override IEnumerable<Expression> equations
  {
    get
    {
      var d0 = _l0.Point0.exp - _l0.Point1.exp;
      var d1 = _l1.Point0.exp - _l1.Point1.exp;

      var angle = ConstraintExp.Angle2d(d0, d1);
      switch (option)
      {
        case Option.LeftHand:
          yield return angle - Math.PI / 2.0;
          break;
        case Option.RightHand:
          yield return angle + Math.PI / 2.0;
          break;
      }
    }
  }
}
