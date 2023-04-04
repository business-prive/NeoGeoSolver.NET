using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class LineCircleDistance : Value
{
  public Line line
  {
    get
    {
      return (Line) GetEntity(0);
    }
    set
    {
      SetEntity(0, value);
    }
  }

  public Circle circle
  {
    get
    {
      return (Circle) GetEntity(1);
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
      return line.p0.exp;
    }
  }

  public ExpressionVector lineP1Exp
  {
    get
    {
      return line.p1.exp;
    }
  }

  public enum Option
  {
    Positive,
    Negative
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
          yield return ConstraintExp.PointLineDistance(centerExp, lineP0Exp, lineP1Exp) - circle.Radius() - value;
          break;
        case Option.Negative:
          yield return ConstraintExp.PointLineDistance(centerExp, lineP0Exp, lineP1Exp) + circle.Radius() + value;
          break;
      }
    }
  }
}
