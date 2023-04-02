using System.Numerics;
using NeoGeoSolver.NET.Constraints;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public abstract partial class Entity : IEntity
{
  protected List<Constraint> usedInConstraints = new List<Constraint>();
  private List<Entity> children = new List<Entity>();
  public Entity parent { get; private set; }
  public Func<ExpressionVector, ExpressionVector> transform = null;
	public IEnumerable<Constraint> constraints { get { return usedInConstraints.AsEnumerable(); } }
  public virtual IEnumerable<Param> parameters { get { yield break; } }
	public virtual IEnumerable<Point> points { get { yield break; } }
  public virtual IEnumerable<Expression> equations { get { yield break; } }

  public abstract IEntityType type { get; }

  IEnumerable<ExpressionVector> IEntity.points
  {
    get
    {
      for (var it = points.GetEnumerator(); it.MoveNext();)
      {
        yield return it.Current.exp;
      }
    }
  }

  public virtual IEnumerable<Vector3> segments
  {
    get
    {
      if (this is ISegmentaryEntity) return (this as ISegmentaryEntity).segmentPoints;
      if (this is ILoopEntity) return (this as ILoopEntity).loopPoints;
      return Enumerable.Empty<Vector3>();
    }
  }

  protected IEnumerable<Vector3> getSegmentsUsingPointOn(int subdiv)
  {
    Param pOn = new Param("pOn");
    var on = PointOn(pOn);
    for (int i = 0; i <= subdiv; i++)
    {
      pOn.value = (double) i / subdiv;
      yield return on.Eval();
    }
  }

  protected IEnumerable<Vector3> getSegments(int subdiv, Func<double, Vector3> pointOn)
  {
    for (int i = 0; i <= subdiv; i++)
    {
      yield return pointOn((double) i / subdiv);
    }
  }

  public abstract ExpressionVector PointOn(Expression t);

  public T AddChild<T>(T e) where T : Entity
  {
    children.Add(e);
    e.parent = this;
    return e;
  }

  public virtual ExpressionVector TangentAt(Expression t)
  {
    Param p = new Param("pOn");
    var pt = PointOn(p);
    var result = new ExpressionVector(pt.x.Deriv(p), pt.y.Deriv(p), pt.z.Deriv(p));
    result.x.Substitute(p, t);
    result.y.Substitute(p, t);
    result.z.Substitute(p, t);
    return result;
  }

  public abstract Expression Length();
  public abstract Expression Radius();

  public virtual ExpressionVector Center()
  {
    return null;
  }
}
