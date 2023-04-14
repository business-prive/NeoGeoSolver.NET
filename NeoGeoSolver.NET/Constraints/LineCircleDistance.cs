using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class LineCircleDistance : Value
{
  private readonly Line _line;
  private readonly Circle _circle;

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
    _line = line;
    _circle = circle;
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
          yield return ConstraintExp.PointLineDistance(_circle.CentreExpr(), _line.Point0.Expr, _line.Point1.Expr) - _circle.RadiusExpr() - value;
          break;
        case Option.Negative:
          yield return ConstraintExp.PointLineDistance(_circle.CentreExpr(), _line.Point0.Expr, _line.Point1.Expr) + _circle.RadiusExpr() + value;
          break;
      }
    }
  }

  public override IEnumerable<Entity> Entities
  {
    get
    {
      yield return _line;
      yield return _circle;
    }
  }
}
