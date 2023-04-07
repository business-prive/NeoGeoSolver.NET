using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class EqualValue : Value
{
  private readonly Value _val0;
  private readonly Value _val1;

  public EqualValue(Value c0, Value c1)
  {
    _val0 = c0;
    _val1 = c1;
  }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return _val0.GetValueParam().Expr - _val1.GetValueParam().Expr - value;
    }
  }
}
