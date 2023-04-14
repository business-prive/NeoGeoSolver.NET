using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class LinesEqualLength : Value
{
  private readonly Line _line0;
  private readonly Line _line1;

  public LinesEqualLength(Line line0, Line line1)
  {
    _line0 = line0;
    _line1 = line1;
    value.Value = 1.0;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return _line0.LengthExpr() - _line1.LengthExpr() * value;
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
