using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public abstract class Entity : IEntity
{
  public virtual IEnumerable<Param> Parameters
  {
    get
    {
      yield break;
    }
  }

  public abstract IEnumerable<Expression> Equations { get; }
}
