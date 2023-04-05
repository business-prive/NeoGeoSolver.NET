using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class LineCircleDistance : Value
{
  public Line Line { get; }

  public Circle Circle { get; }

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
    Line = line;
    Circle = circle;
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
          yield return ConstraintExp.PointLineDistance(Circle.CentreExpr(), Line.p0.exp, Line.p1.exp) - Circle.RadiusExpr() - value;
          break;
        case Option.Negative:
          yield return ConstraintExp.PointLineDistance(Circle.CentreExpr(), Line.p0.exp, Line.p1.exp) + Circle.RadiusExpr() + value;
          break;
      }
    }
  }
}
