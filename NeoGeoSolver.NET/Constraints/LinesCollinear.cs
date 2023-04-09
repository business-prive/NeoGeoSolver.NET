using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class LinesCollinear : Constraint
{
  private readonly Line _line0;
  private readonly Line _line1;

  private readonly Parallel _parallel;
  private readonly PointLineDistance _distL1_L2P0;
  private readonly PointLineDistance _distL1_L2P1;

  public LinesCollinear(Line line0, Line line1)
  {
    _line0 = line0;
    _line1 = line1;

    _parallel = new Parallel(_line0, _line1);
    _distL1_L2P0 = new PointLineDistance(_line1.Point0, _line0);
    _distL1_L2P0.SetValue(0);
    _distL1_L2P1 = new PointLineDistance(_line1.Point1, _line0);
    _distL1_L2P1.SetValue(0);
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      foreach (var expr in _parallel.Equations)
      {
        yield return expr;
      }
      foreach (var expr in _distL1_L2P0.Equations)
      {
        yield return expr;
      }
      foreach (var expr in _distL1_L2P1.Equations)
      {
        yield return expr;
      }
    }
  }
}
