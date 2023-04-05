using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class LineCircleDistance : Value
{
  public Line line
  {
    get;set;
  }

  public Circle circle
  {
    get;set;
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

  public LineCircleDistance(Line line, Circle circle)
  {
    this.line = line;
    this.circle = circle;
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
