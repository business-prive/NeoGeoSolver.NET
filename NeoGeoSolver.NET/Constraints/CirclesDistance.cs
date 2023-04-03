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

  private Option option_;

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

  public CirclesDistance()
  {
  }

  public CirclesDistance(IEntity c0, IEntity c1)
  {
    AddEntity(c0);
    AddEntity(c1);
    value.value = 1;
    ChooseBestOption();
    Satisfy();
  }

  private Point getCenterPoint(IEntity e)
  {
    if (e is Circle) return (e as Circle).center;
    if (e is Arc) return (e as Arc).Center;
    return null;
  }

  private bool isCentersCoincident(IEntity c0, IEntity c1)
  {
    var cp0 = getCenterPoint(c0);
    var cp1 = getCenterPoint(c1);
    return cp0 != null && cp1 != null && cp0.IsCoincidentWith(cp1);
  }

  public override IEnumerable<Expression> equations
  {
    get
    {
      var c0 = GetEntity(0);
      var c1 = GetEntity(1);
      var r0 = c0.Radius();
      var r1 = c1.Radius();
      if (isCentersCoincident(c0, c1))
      {
        if (option == Option.FirstInside)
        {
          yield return r0 - r1 - value.exp;
        }
        else
        {
          yield return r1 - r0 - value.exp;
        }
      }
      else
      {
        var dist = (c0.CenterInPlane(sketch.plane) - c1.CenterInPlane(sketch.plane)).Magnitude();
        switch (option)
        {
          case Option.Outside:
            yield return (dist - r0 - r1) - value.exp;
            break;
          case Option.FirstInside:
            yield return (r1 - r0 - dist) - value.exp;
            break;
          case Option.SecondInside:
            yield return (r0 - r1 - dist) - value.exp;
            break;
        }
      }
    }
  }
}
