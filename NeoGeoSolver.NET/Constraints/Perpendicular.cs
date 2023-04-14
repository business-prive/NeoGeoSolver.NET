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

  private readonly Line _line0;
  private readonly Line _line1;

  public Perpendicular(Line line0, Line line1)
  {
    _line0 = line0;
    _line1 = line1;
    ChooseBestOption();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var d0 = _line0.Point0.Expr - _line0.Point1.Expr;
      var d1 = _line1.Point0.Expr - _line1.Point1.Expr;

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

  public override IEnumerable<Entity> Entities
  {
    get
    {
      yield return _line0;
      yield return _line1;
    }
  }
}
