using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public abstract class Entity : IEntity
{
  public virtual IEnumerable<Param> parameters { get { yield break; } }
  public abstract IEnumerable<Expression> equations { get; }

  public abstract ExpressionVector PointOn(Expression t);

  public virtual ExpressionVector TangentAt(Expression t)
  {
    var p = new Param("pOn");
    var pt = PointOn(p);
    var result = new ExpressionVector(pt.x.Deriv(p), pt.y.Deriv(p), pt.z.Deriv(p));
    result.x.Substitute(p, t);
    result.y.Substitute(p, t);
    result.z.Substitute(p, t);
    return result;
  }
}
