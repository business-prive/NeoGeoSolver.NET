using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Constraints;

public class ArcAngle : Value
{
  private bool _supplementary;

  public bool Supplementary
  {
    get
    {
      return _supplementary;
    }
    set
    {
      if (value == _supplementary)
      {
        return;
      }

      Supplementary = value;
      this.value.Value = 2.0 * Math.PI - this.value.Value;
    }
  }

  private readonly Arc _arc;

  public ArcAngle(Arc arc)
  {
    _arc = arc;
    value.Value = Math.PI / 4;
    Satisfy();
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      var angle = _arc.EndAngle.Expr - _arc.StartAngle.Expr;
      yield return angle - value;
    }
  }

  public override IEnumerable<Entity> Entities
  {
    get
    {
      yield return _arc;
    }
  }
}
