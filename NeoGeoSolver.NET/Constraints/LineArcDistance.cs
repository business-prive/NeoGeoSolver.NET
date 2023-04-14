using System.Xml.Serialization;
using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class LineArcDistance : Value
{
  private readonly Line _line;
  private readonly Arc _arc;

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

  public LineArcDistance(Line line, Arc arc)
  {
    _line = line;
    _arc = arc;
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
          yield return ConstraintExp.PointLineDistance(_arc.Centre.Expr, _line.Point0.Expr, _line.Point1.Expr) - _arc.Radius.Expr - value;
          break;
        case Option.Negative:
          yield return ConstraintExp.PointLineDistance(_arc.Centre.Expr, _line.Point0.Expr, _line.Point1.Expr) + _arc.Radius.Expr + value;
          break;
      }
    }
  }

  public override IEnumerable<Entity> Entities
  {
    get
    {
      yield return _line;
      yield return _arc;
    }
  }
}
