using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class ArcsEqualRadius : Constraint
{
    private readonly Arc _arc0;
    private readonly Arc _arc1;

    public ArcsEqualRadius(Arc arc0, Arc arc1)
    {
        _arc0 = arc0;
        _arc1 = arc1;
    }

  public override IEnumerable<Expression> Equations
  {
    get
    {
      yield return _arc0.Radius.Expr - _arc1.Radius.Expr;
    }
  }
}
