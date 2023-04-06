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

  public override IEnumerable<Expression> Equations
  {
    get
    {
      switch (option)
      {
        case Option.Positive:
          yield return ConstraintExp.PointLineDistance(Circle.CentreExpr(), Line.Point0.Expr, Line.Point1.Expr) - Circle.RadiusExpr() - value;
          break;
        case Option.Negative:
          yield return ConstraintExp.PointLineDistance(Circle.CentreExpr(), Line.Point0.Expr, Line.Point1.Expr) + Circle.RadiusExpr() + value;
          break;
      }
    }
  }
}
