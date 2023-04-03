using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class LineCircleDistance : Value
{
  public IEntity line
  {
    get
    {
      return GetEntity(0);
    }
    set
    {
      SetEntity(0, value);
    }
  }

  public IEntity circle
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

  public ExpressionVector centerExp
  {
    get
    {
      return circle.Center();
    }
  }

  public ExpressionVector lineP0Exp
  {
    get
    {
      return line.PointsInPlane().ToArray()[0];
    }
  }

  public ExpressionVector lineP1Exp
  {
    get
    {
      return line.PointsInPlane().ToArray()[1];
    }
  }

  public enum Option
  {
    Positive,
    Negative
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

  public LineCircleDistance(IEntity line, IEntity circle)
  {
    AddEntity(line);
    AddEntity(circle);
    SetValue(1.0);
    ChooseBestOption();
    Satisfy();
  }

  public override IEnumerable<Expression> equations
  {
    get
    {
      switch (option)
      {
        case Option.Positive:
          yield return ConstraintExp.pointLineDistance(centerExp, lineP0Exp, lineP1Exp) - circle.Radius() - value;
          break;
        case Option.Negative:
          yield return ConstraintExp.pointLineDistance(centerExp, lineP0Exp, lineP1Exp) + circle.Radius() + value;
          break;
      }
    }
  }
}
