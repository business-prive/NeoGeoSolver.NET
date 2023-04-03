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

  public Perpendicular(IEntity l0, IEntity l1)
  {
    AddEntity(l0);
    AddEntity(l1);
    ChooseBestOption();
  }

  public override IEnumerable<Expression> equations
  {
    get
    {
      var l0 = GetEntityOfType(EntityType.Line, 0);
      var l1 = GetEntityOfType(EntityType.Line, 1);

      var d0 = l0.GetPointAtInPlane(0) - l0.GetPointAtInPlane(1);
      var d1 = l1.GetPointAtInPlane(0) - l1.GetPointAtInPlane(1);

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
